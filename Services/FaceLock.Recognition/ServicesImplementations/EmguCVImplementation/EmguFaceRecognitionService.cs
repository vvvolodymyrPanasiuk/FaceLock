using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV;
using FaceLock.Recognition.DTO;
using FaceLock.Recognition.Services;
using System.Drawing;
using Emgu.CV.Util;
using Newtonsoft.Json;
using FaceLock.Recognition.RecognitionSettings;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace FaceLock.Recognition.ServicesImplementations.EmguCVImplementation
{
    /// <summary>
    /// This class provides face recognition functionality using the EmguCV library.
    /// </summary>
    /// <typeparam name="T">The type of the user ID.</typeparam>
    public class EmguFaceRecognitionService<T> : IFaceRecognitionService<T>
    {
        private const double FaceRecognitionThreshold = 5000;
        private readonly FaceRecognizer faceRecognizer;
        private readonly CascadeClassifier faceDetector;
        private readonly string vectorOfIntJsonFilePath;
        private readonly string vectorOfMatJsonFilePath;
        private readonly string userIdToLabelMapJsonFilePath;
        private readonly string labelToUserIdMapJsonFilePath;
        private readonly string emguTrainingModelFilePath;
        
        private Dictionary<T, int>? userIdToLabelMap;
        private Dictionary<int, T>? labelToUserIdMap;
        private VectorOfInt? vectorOfInt;
        private VectorOfMat? vectorOfMat;

        public EmguFaceRecognitionService(IOptions<EmguCVFaceRecognationSettings> appSettings)
        {
            vectorOfIntJsonFilePath = appSettings.Value.vectorOfIntJsonFilePath;
            vectorOfMatJsonFilePath = appSettings.Value.vectorOfMatJsonFilePath;
            userIdToLabelMapJsonFilePath = appSettings.Value.userIdToLabelMapJsonFilePath;
            labelToUserIdMapJsonFilePath = appSettings.Value.labelToUserIdMapJsonFilePath;
            emguTrainingModelFilePath = appSettings.Value.emguTrainingModelFilePath;

            faceRecognizer = new LBPHFaceRecognizer(1, 8, 8, 8, FaceRecognitionThreshold);
            faceDetector = new CascadeClassifier(appSettings.Value.cascadeClassifierFilePath);
        }


        #region Recognize

        public async Task<FaceRecognitionResult<T>> RecognizeUserByFaceAsync(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                throw new ArgumentNullException(nameof(imageData), "File error (empty)");
            }

            using var image = new Bitmap(new MemoryStream(imageData)).ToImage<Gray, byte>();
            var detectedFace = DetectedFace.DetectFace(image, faceDetector);

            if (detectedFace == null)
            {
                throw new Exception("Face not detected.");
            }

            var result = new FaceRecognitionResult<T>();

            faceRecognizer.Read(emguTrainingModelFilePath);
            var predictionResult = faceRecognizer.Predict(detectedFace.FaceImage);

            labelToUserIdMap = LoadDictionaryFromJson<int, T>(labelToUserIdMapJsonFilePath) ?? new Dictionary<int, T>();
            if(labelToUserIdMap == null)
            {
                throw new Exception("Training model error (empty).");
            }
            if (labelToUserIdMap.TryGetValue(predictionResult.Label, out T userId))
            {
                result.UserId = userId;
                result.PredictionDistance = predictionResult.Distance;
            }

            return result;
        }

        public async Task<FaceRecognitionResult<T>> RecognizeUserByFacesAsync(List<byte[]> imagesData)
        {
            if (imagesData == null || imagesData.Count == 0)
            {
                throw new ArgumentNullException(nameof(imagesData), "File error (empty)");
            }

            FaceRecognitionResult<T> result = new FaceRecognitionResult<T>();

            foreach (var imageData in imagesData)
            {
                //TODO: check distance of prediction

                result = await RecognizeUserByFaceAsync(imageData);
            }

            return result;
        }

        public async Task<FaceRecognitionResult<T>> RecognizeUserByFaceAsync(IFormFile imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                throw new ArgumentNullException(nameof(imageData), "File error (empty)");
            }

            FaceRecognitionResult<T> result = new FaceRecognitionResult<T>();

            using (var memoryStream = new MemoryStream())
            {
                await imageData.CopyToAsync(memoryStream);
                var ArrayFace = memoryStream.ToArray();

                result = await RecognizeUserByFaceAsync(ArrayFace);
            }

            return result;
        }

        public async Task<FaceRecognitionResult<T>> RecognizeUserByFacesAsync(IFormFileCollection imagesData)
        {
            if (imagesData == null || imagesData.Count == 0)
            {
                throw new ArgumentNullException(nameof(imagesData), "File error (empty)");
            }

            FaceRecognitionResult<T> result = new FaceRecognitionResult<T>();

            foreach (var image in imagesData)
            {
                //TODO: check distance of prediction

                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    var ArrayFace = memoryStream.ToArray();

                    result = await RecognizeUserByFaceAsync(ArrayFace);
                }
            }

            return result;
        }

        #endregion


        #region Training model

        public async Task AddUserFaceToTrainModelAsync(T userId, byte[] imageData)
        {
            if (imageData == null)
            {
                throw new ArgumentNullException(nameof(imageData), "File error (empty)");
            }
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId), "User ID error (empty)");
            }

            using var image = new Bitmap(new MemoryStream(imageData)).ToImage<Gray, byte>();
            var detectedFace = DetectedFace.DetectFace(image, faceDetector);

            if (detectedFace == null)
            {
                throw new Exception("Face not detected.");
            }

            userIdToLabelMap = LoadDictionaryFromJson<T, int>(userIdToLabelMapJsonFilePath) ?? new Dictionary<T, int>();
            labelToUserIdMap = LoadDictionaryFromJson<int, T>(labelToUserIdMapJsonFilePath) ?? new Dictionary<int, T>();

            if (!userIdToLabelMap.TryGetValue(userId, out int label))
            {
                label = userIdToLabelMap.Count;
                userIdToLabelMap.Add(userId, label);
                labelToUserIdMap.Add(label, userId);
            }

            vectorOfMat = LoadVectorFromJson<VectorOfMat>(vectorOfMatJsonFilePath) ?? new VectorOfMat();
            vectorOfInt = LoadVectorFromJson<VectorOfInt>(vectorOfIntJsonFilePath) ?? new VectorOfInt();

            List<Image<Gray, byte>> TrainedFaces = new List<Image<Gray, byte>>();
            TrainedFaces.Add(detectedFace.FaceImage);
            Image<Gray, byte>[] Faces = TrainedFaces.ToArray();
            vectorOfMat.Push(Faces);

            List<int> PersonsLabes = new List<int>();
            PersonsLabes.Add(label);
            int[] labels = PersonsLabes.ToArray();
            vectorOfInt.Push(labels);

            faceRecognizer.Train(vectorOfMat, vectorOfInt);

            // Save data to JSON files
            SaveVectorToJson(vectorOfMat, vectorOfMatJsonFilePath);
            SaveVectorToJson(vectorOfInt, vectorOfIntJsonFilePath);
            SaveDictionaryToJson<T, int>(userIdToLabelMap, userIdToLabelMapJsonFilePath);
            SaveDictionaryToJson<int, T>(labelToUserIdMap, labelToUserIdMapJsonFilePath);
            faceRecognizer.Write(emguTrainingModelFilePath);
        }

        public async Task AddUserFacesToTrainModelAsync(T userId, List<byte[]> imagesData)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId), "User ID error (empty)");
            }
            if (imagesData == null)
            {
                throw new ArgumentNullException(nameof(imagesData), "File error (empty)");
            }

            foreach (var imageData in imagesData)
            {
                await AddUserFaceToTrainModelAsync(userId, imageData);
            }
        }

        public async Task AddUserFaceToTrainModelAsync(T userId, IFormFile imageData)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId), "User ID error (empty)");
            }
            if (imageData == null)
            {
                throw new ArgumentNullException(nameof(imageData), "File error (empty)");
            }

            using (var memoryStream = new MemoryStream())
            {
                await imageData.CopyToAsync(memoryStream);
                var ArrayFace = memoryStream.ToArray();

                await AddUserFaceToTrainModelAsync(userId, ArrayFace);
            }
        }

        public async Task AddUserFacesToTrainModelAsync(T userId, IFormFileCollection imagesData)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId), "User ID error (empty)");
            }
            if (imagesData == null)
            {
                throw new ArgumentNullException(nameof(imagesData), "File error (empty)");
            }

            foreach(var imageData in imagesData)
            {
                await AddUserFaceToTrainModelAsync(userId, imageData);
            }
        }

        public async Task RemoveUserFacesFromTrainModelAsync(T userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId), "User ID error (empty)");
            }

            userIdToLabelMap = LoadDictionaryFromJson<T, int>(userIdToLabelMapJsonFilePath) ?? new Dictionary<T, int>();
            labelToUserIdMap = LoadDictionaryFromJson<int, T>(labelToUserIdMapJsonFilePath) ?? new Dictionary<int, T>();

            if (userIdToLabelMap.TryGetValue(userId, out int label))
            {
                userIdToLabelMap.Remove(userId);
                labelToUserIdMap.Remove(label);

                SaveDictionaryToJson(userIdToLabelMap, userIdToLabelMapJsonFilePath);
                SaveDictionaryToJson(labelToUserIdMap, labelToUserIdMapJsonFilePath);

                // Update the training data
                vectorOfMat = LoadVectorFromJson<VectorOfMat>(vectorOfMatJsonFilePath) ?? new VectorOfMat();
                vectorOfInt = LoadVectorFromJson<VectorOfInt>(vectorOfIntJsonFilePath) ?? new VectorOfInt();

                var labels = vectorOfInt.ToArray();
                var newLabels = labels.Where(l => l != label).ToArray();

                var newVectorOfMat = new VectorOfMat();
                var newVectorOfInt = new VectorOfInt();

                for (int i = 0; i < vectorOfMat.Size; i++)
                {
                    if (labels[i] != label)
                    {
                        newVectorOfMat.Push(vectorOfMat[i]);
                    }
                }
                newVectorOfInt.Push(newLabels);

                vectorOfMat = newVectorOfMat;
                vectorOfInt = newVectorOfInt;

                // Re-train the recognizer with the updated data
                faceRecognizer.Train(vectorOfMat, vectorOfInt);

                // Save updated data to JSON files
                SaveVectorToJson(vectorOfMat, vectorOfMatJsonFilePath);
                SaveVectorToJson(vectorOfInt, vectorOfIntJsonFilePath);
                faceRecognizer.Write(emguTrainingModelFilePath);
            }
        }

        #endregion


        #region Additional methods

        /// <summary>
        /// Serializes a VectorOf<T> to JSON and saves it to a file.
        /// </summary>
        /// <typeparam name="TVector">The type of the vector.</typeparam>
        /// <param name="vector">The vector to serialize.</param>
        /// <param name="filePath">The file path to save the JSON.</param>
        private void SaveVectorToJson<TVector>(TVector vector, string filePath)
        {
            // If the vector is VectorOfMat, convert it to List<Mat> and then serialize
            if (vector is VectorOfMat vectorOfMat)
            {
                List<Mat> matList = new List<Mat>();
                for (int i = 0; i < vectorOfMat.Size; i++)
                {
                    matList.Add(vectorOfMat[i]);
                }
                string json = JsonConvert.SerializeObject(matList, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            else
            {
                // For other types of vectors, serialize directly
                string json = JsonConvert.SerializeObject(vector, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
        }

        /// <summary>
        /// Deserializes a VectorOf<T> from JSON and loads it from a file.
        /// </summary>
        /// <typeparam name="TVector">The type of the vector.</typeparam>
        /// <param name="filePath">The file path to load the JSON from.</param>
        /// <returns>The deserialized vector.</returns>
        private TVector LoadVectorFromJson<TVector>(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                // If the vector is VectorOfMat, convert from List<Mat> to VectorOfMat
                if (typeof(TVector) == typeof(VectorOfMat))
                {
                    var matList = JsonConvert.DeserializeObject<List<Mat>>(json);
                    if(matList == null)
                    {
                        return default;
                    }

                    var vectorOfMat = new VectorOfMat(matList.ToArray());

                    return (TVector)(object)vectorOfMat;
                }
                else
                {
                    return JsonConvert.DeserializeObject<TVector>(json);
                }
            }
            return default;
        }

        /// <summary>
        /// Serializes a dictionary to JSON and saves it to a file.
        /// </summary>
        /// <typeparam name="TDictionaryKey">The type of the dictionary keys.</typeparam>
        /// <typeparam name="TDictionaryValue">The type of the dictionary values.</typeparam>
        /// <param name="dictionary">The dictionary to serialize.</param>
        /// <param name="filePath">The file path to save the JSON.</param>
        private void SaveDictionaryToJson<TDictionaryKey, TDictionaryValue>(Dictionary<TDictionaryKey, TDictionaryValue> dictionary, string filePath)
        {
            string json = JsonConvert.SerializeObject(dictionary);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Deserializes a dictionary from JSON and loads it from a file.
        /// </summary>
        /// <typeparam name="TDictionaryKey">The type of the dictionary keys.</typeparam>
        /// <typeparam name="TDictionaryValue">The type of the dictionary values.</typeparam>
        /// <param name```csharp
        /// <typeparam name="TDictionaryKey">The type of the dictionary keys.</typeparam>
        /// <typeparam name="TDictionaryValue">The type of the dictionary values.</typeparam>
        /// <param name="filePath">The file path to load the JSON from.</param>
        /// <returns>The deserialized dictionary.</returns>
        private Dictionary<TDictionaryKey, TDictionaryValue> LoadDictionaryFromJson<TDictionaryKey, TDictionaryValue>(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                if(json == null)
                {
                    return new Dictionary<TDictionaryKey, TDictionaryValue>();
                }

                return JsonConvert.DeserializeObject<Dictionary<TDictionaryKey, TDictionaryValue>>(json);
            }
            return null;
        }

        #endregion
    }
}
