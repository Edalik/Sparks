using Alturos.Yolo;
using Alturos.Yolo.Model;
using Microsoft.Win32;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using Sparks.Modules.Main.Models.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Sparks.Modules.Main.Services;

internal class MainService : IMainService
{
    YoloWrapper yoloWrapper { get; set; } = new YoloWrapper("yolov3.cfg", "yolov3_training_final.weights", "coco.names");

    public Task ReadFile(IMainModel mainModel, CancellationToken cancellationToken)
    {
        return Task.Run
        (
            async () =>
            {
                try
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    if (openFileDialog.ShowDialog() == false)
                    {
                        return;
                    }
                    VideoCapture videoCapture = new VideoCapture(openFileDialog.FileName);
                    Mat mat = new Mat();
                    int consecutiveFrames = 0;
                    int fps = (int)videoCapture.Fps;
                    for (int i = 0; i < videoCapture.FrameCount && !cancellationToken.IsCancellationRequested; i++)
                    {
                        videoCapture.Read(mat);
                        List<YoloItem> yoloItems = yoloWrapper.Detect(mat.ToBytes().ToArray()).ToList<YoloItem>();
                        foreach (YoloItem yoloItem in yoloItems)
                        {
                            if (yoloItem.Confidence > 0.75)
                            {
                                mat.Rectangle(new OpenCvSharp.Point(yoloItem.X, yoloItem.Y), new OpenCvSharp.Point(yoloItem.X + yoloItem.Width, yoloItem.Y + yoloItem.Height), Scalar.Green, 3);
                                mat.PutText(Math.Round(yoloItem.Confidence, 2).ToString(), new OpenCvSharp.Point(yoloItem.X, yoloItem.Y), HersheyFonts.HersheySimplex, 2, Scalar.Green, 4);
                            }
                        }
                        int x = 0;
                        Mat matg = mat.Clone();
                        Cv2.CvtColor(matg, matg, ColorConversionCodes.BGR2GRAY);
                        Cv2.AdaptiveThreshold(matg, matg, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, 21, -75);
                        Cv2.FindContours(matg, out OpenCvSharp.Point[][] contours, out HierarchyIndex[] hierarchyIndexes, mode: RetrievalModes.List, method: ContourApproximationModes.ApproxNone);
                        for (int j = 0; j < contours.Length; j++)
                        {
                            if (contours[j].Length < 2)
                            {
                                x++;
                            }
                        }
                        mainModel.SparksCount = x.ToString();
                        if (x >= 50)
                            consecutiveFrames++;
                        else
                            consecutiveFrames = 0;
                        mainModel.Conclusion = consecutiveFrames >= fps ? "Выпуск продут" : "Выпуск не продут";
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            mainModel.Frame = mat.ToBitmapSource();
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        );
    }
}
