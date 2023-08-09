using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Drawing;

namespace FaceLock.Recognition.ServicesImplementations.EmguCVImplementation
{
    /// <summary>
    /// Represents a detected face.
    /// </summary> 
    public class DetectedFace
    {
        /// <summary>
        /// Gets or sets the image of the detected face.
        /// </summary>
        public Image<Gray, byte> FaceImage { get; set; }


        #region Methods

        /// <summary>
        /// Detects a single face in the given image using the specified face detector.
        /// </summary>
        /// <param name="image">The image to detect the face in.</param>
        /// <param name="faceDetector">The face detector to use.</param>
        /// <returns>The detected face.</returns>
        public static DetectedFace DetectFace(Image<Gray, byte> image, CascadeClassifier faceDetector)
        {
            // Detect faces using the HaarCascadeClassifier
            var faces = faceDetector.DetectMultiScale(image, 1.1, 3, Size.Empty);
            if(faces.Length == 0)
            {
                throw new ArgumentNullException("Face not detected.");
            }

            var detectedFaces = new DetectedFace();
            // Extract first detected face region and convert it to grayscale image
            detectedFaces.FaceImage = image.Copy(faces[0]).Resize(100, 100, Inter.Cubic);            

            return detectedFaces;
        }

        /// <summary>
        /// Detects multiple faces in the given image using the specified face detector.
        /// </summary>
        /// <param name="image">The image to detect the faces in.</param>
        /// <param name="faceDetector">The face detector to use.</param>
        /// <returns>An array of detected faces.</returns>
        public static DetectedFace[] DetectFaces(Image<Gray, byte> image, CascadeClassifier faceDetector)
        {
            // Detect faces using the HaarCascadeClassifier
            var faces = faceDetector.DetectMultiScale(image, 1.1, 3, Size.Empty);
            if (faces.Length == 0)
            {
                throw new ArgumentNullException("Face not detected.");
            }

            // Create an array to store the detected faces
            var detectedFaces = new List<DetectedFace>();

            // Extract each detected face region and convert it to grayscale image
            foreach (var faceRect in faces)
            {
                var faceImage = image.Copy(faceRect).Resize(100, 100, Inter.Cubic);
                detectedFaces.Add(new DetectedFace { FaceImage = faceImage });
            }

            return detectedFaces.ToArray();
        }

        /// <summary>
        /// Detects multiple faces in the given image using the specified face detector asynchronously.
        /// </summary>
        /// <param name="image">The image to detect the faces in.</param>
        /// <param name="faceDetector">The face detector to use.</param>
        /// <returns>An array of detected faces.</returns>
        private static async Task<DetectedFace[]> DetectFacesAsync(Image<Gray, byte> image, CascadeClassifier faceDetector)
        {
            // Detect faces using the HaarCascadeClassifier
            var faces = await Task.Run(() => faceDetector.DetectMultiScale(image, 1.1, 3, Size.Empty));
            if (faces.Length == 0)
            {
                throw new ArgumentNullException("Face not detected.");
            }

            // Create an array to store the detected faces
            var detectedFaces = new List<DetectedFace>();

            // Extract each detected face region and convert it to grayscale image
            foreach (var faceRect in faces)
            {
                var faceImage = image.Copy(faceRect).Resize(100, 100, Inter.Cubic);
                detectedFaces.Add(new DetectedFace { FaceImage = faceImage });
            }

            return detectedFaces.ToArray();
        }

        #endregion
    }
}
