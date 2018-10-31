// Copyright (c) 2018 stefan.v
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// // copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using UnityEngine;

namespace Demo
{
	/// <summary>
	/// Segment which holds obstacles.
	/// </summary>
	public class TrackSegment : MonoBehaviour
	{
		/// <summary>
		/// Horizontal extent in spot matrix units.
		/// </summary>
		public const int WIDTH = 20;
		/// <summary>
		/// Vertical extent in spot matrix units.
		/// </summary>
		public const int LENGTH = 50;

		/// <summary>
		/// Central location at segment's end.
		/// </summary>
		[SerializeField]
		private Transform endMarker;

		/// <summary>
		/// Position of central location at segment's end.
		/// </summary>
		public Vector3 EndMarkerPos { get { return endMarker.position; } }

		/// <summary>
		/// Computes a world position based on occupancy matrix location.
		/// </summary>
		/// <param name="i">Column.</param>
		/// <param name="j">Row.</param>
		/// <returns>Position on segment in world space.</returns>
		public Vector3 GetSpotPos(int i, int j)
		{
			//get local position shifted to the center of the cell
			Vector3 local = new Vector3(i - WIDTH / 2, 0f, j) + (Vector3.right + Vector3.forward) / 2f;
			//return world correspondent
			return transform.TransformPoint(local);
		}
	}
}