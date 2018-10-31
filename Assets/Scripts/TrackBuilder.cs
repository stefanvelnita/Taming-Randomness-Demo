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

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Demo
{
	/// <summary>
	/// Lays down segments of track and calls placers to add obstacles on them.
	/// </summary>
	public class TrackBuilder : MonoBehaviour
	{
		[Range(1, 100)]
		public int segmentsToAdd;

		/// <summary>
		/// Where to place the created segments.
		/// </summary>
		[SerializeField]
		private Transform segmentsContainer;

		/// <summary>
		/// Segment prefab.
		/// </summary>
		[SerializeField]
		private TrackSegment segmentAsset;

		/// <summary>
		/// Cache currently only used for cleaning.
		/// </summary>
		[SerializeField]
		private List<TrackSegment> segments;

		/// <summary>
		/// Will be invoked for every segment. Order matters.
		/// </summary>
		[SerializeField]
		private Placer[] placers;

		/// <summary>
		/// Reflects segment occupancy. Single instance per build.
		/// </summary>
		private int[,] spots;

		/// <summary>
		/// Lays segment of track and ads assets to them.
		/// </summary>
		public void Build()
		{
			Clear();

			#if UNITY_EDITOR
				if(!EditorApplication.isPlaying)
					Undo.RecordObject(this, "Create World");
			#endif

			Init();			
			Vector3 position = Vector3.zero;

			for(int i = 0; i < segmentsToAdd; i++)
			{
				TrackSegment segment = Instantiate(segmentAsset, position, Quaternion.identity, segmentsContainer);

				#if UNITY_EDITOR
					if(!EditorApplication.isPlaying)
						Undo.RegisterCreatedObjectUndo(segment.gameObject, "Create Track");
				#endif

				segments.Add(segment);
				//prepare for next
				position = segment.EndMarkerPos;

				//populate
				for(int p = 0; p < placers.Length; p++)
					placers[p].Place(segment, spots);

				ClearSpots();
			}

			UnInit();
		}

		/// <summary>
		/// Removes all track segment objects.
		/// </summary>
		public void Clear()
		{
			if(segments == null)
				return;

			#if UNITY_EDITOR
				if(!EditorApplication.isPlaying)
					Undo.RegisterCompleteObjectUndo(this, "Clear Track");
			#endif

			segments.ForEach(segment => {

				//editor & not playing
				#if UNITY_EDITOR
					if(!EditorApplication.isPlaying)
					{
						Undo.DestroyObjectImmediate(segment.gameObject);
						return;
					}
				#endif

				//play mode & build
				Destroy(segment.gameObject);
			});

			segments.Clear();
			segments = null;
		}

		/// <summary>
		/// Called before build for allocations.
		/// </summary>
		private void Init()
		{
			if(segments == null)
				segments = new List<TrackSegment>(segmentsToAdd);

			spots = new int[TrackSegment.WIDTH, TrackSegment.LENGTH];

			for(int p = 0; p < placers.Length; p++)
				placers[p].Init();
		}

		/// <summary>
		/// Called after build for cleanup.
		/// </summary>
		private void UnInit()
		{
			spots = null;

			for(int p = 0; p < placers.Length; p++)
			placers[p].UnInit();
		}

		private void ClearSpots()
		{
			for(int i = 0; i < TrackSegment.WIDTH; i++)
				for(int j = 0; j < TrackSegment.LENGTH; j++)
					spots[i, j] = 0;
		}
	}
}