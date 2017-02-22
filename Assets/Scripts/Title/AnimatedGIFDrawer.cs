using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using System.Drawing.Imaging;
// needs System.Drawing.dll put in Assets folder of project
//  taken from \Unity 5\Editor\Data\Mono\lib\mono\2.0

public class AnimatedGIFDrawer : MonoBehaviour {

	[Tooltip("Absolute or relative path to file")] public string LoadingGifPath;  //can be absolute or relative (Assets\...) to anim gif file
	//public float speed = 1f;
	[SerializeField] private float framesPerSecond = 10f;
	[SerializeField] private Vector2 drawPosition;
	[SerializeField] private float imageScale = 1.0f;
	[SerializeField] private bool loopClip = true;
	[SerializeField] [Tooltip("Repeat first frame at start")] [Range(0, 1000)] private int bufferFramesStart = 0;
	[SerializeField] [Tooltip("Repeat last frame at end")] [Range(0, 1000)] private int bufferFramesEnd = 0;
	List<Texture2D> gifFrames = new List<Texture2D>();
	private bool hasLooped = false;

	void Awake () {
		var gifImage = Image.FromFile(LoadingGifPath);
		var dimension = new FrameDimension(gifImage.FrameDimensionsList[0]);
		int frameCount = gifImage.GetFrameCount(dimension);
		for (int i=0; i<frameCount; i++) {
			gifImage.SelectActiveFrame(dimension, i);
			var frame = new Bitmap(gifImage.Width, gifImage.Height);
			System.Drawing.Graphics.FromImage(frame).DrawImage(gifImage, Point.Empty);
			var frameTexture = new Texture2D(frame.Width, frame.Height);
			for (int x=0; x<frame.Width; x++) {
				for (int y=0; y<frame.Height; y++) {
					System.Drawing.Color sourceColor = frame.GetPixel(x, y);
					frameTexture.SetPixel(-(frame.Width - 1 - x), -y, new Color32(sourceColor.R, sourceColor.G, sourceColor.B, sourceColor.A));
				}
			}
			frameTexture.Apply();
			gifFrames.Add(frameTexture);
			if ((i == 0) && (bufferFramesStart > 0)) {    //first frame
				for (int j=0; j<bufferFramesStart; j++) {
					gifFrames.Add(frameTexture);
				}
			}
			if ((i == (frameCount - 1)) && (bufferFramesEnd > 0)) {    //last frame
				for (int j=0; j<bufferFramesEnd; j++) {
					gifFrames.Add(frameTexture);
				}
			}
		}
	}

	void OnGUI () {
		if (loopClip || !hasLooped) {
			//GUI.DrawTexture(new Rect(drawPosition.x, drawPosition.y, gifFrames[0].width, gifFrames[0].height), gifFrames[(int)(Time.frameCount * speed) % gifFrames.Count]);
			GUI.DrawTexture(new Rect(drawPosition.x, drawPosition.y, gifFrames[0].width * imageScale, gifFrames[0].height * imageScale), gifFrames[(int)(Time.timeSinceLevelLoad * framesPerSecond) % gifFrames.Count]);
		} else {
			GUI.DrawTexture(new Rect(drawPosition.x, drawPosition.y, gifFrames[0].width * imageScale, gifFrames[0].height * imageScale), gifFrames[gifFrames.Count - 1]);
			//erase this line if image should disappear after one play
		}
		//if ((Time.timeSinceLevelLoad * framesPerSecond) / gifFrames.Count > 1f) 
		if ((int)(Time.timeSinceLevelLoad * framesPerSecond) % gifFrames.Count == (gifFrames.Count - 1))
			{ hasLooped = true; }
	}
}
