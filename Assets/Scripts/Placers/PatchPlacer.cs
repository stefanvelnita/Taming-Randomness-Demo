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
using UnityEngine;

namespace Demo
{
	/// <summary>
	/// Computes and places assets in a group like manner.
	/// </summary>
	public class PatchPlacer : Placer
	{
		/// <summary>
		/// Extent of individual patch.
		/// </summary>
		[SerializeField]
		private int size;

		/// <summary>
		/// How many patches to try to place on segment.
		/// </summary>
		[SerializeField]
		private int targetCount;

		/// <summary>
		/// How many assets to be added to an individual patch.
		/// </summary>
		[SerializeField]
		[Range(0f, 1f)]
		private float targetFillRatio;

		/// <summary>
		/// Should areas be considered if the target fill ratio can be respected or fill only complete empty ones.
		/// </summary>
		[SerializeField]
		private bool allowIntertwined = true;

		/// <summary>
		/// Computed only once.
		/// </summary>
		private int assetsPerPatch;

		/// <summary>
		/// Possible areas for the segment.
		/// </summary>
		private List<PlacerArea> areas;

		/// <summary>
		/// List of free spots in current area.
		/// </summary>
		private List<PlacerSpot> free;

		public override void Init()
		{
			base.Init();

			assetsPerPatch = (int)((size * size) * targetFillRatio);

			//initialise lists only one per build process
			areas = new List<PlacerArea>((TrackSegment.WIDTH - size) * (TrackSegment.LENGTH - size));
			free = new List<PlacerSpot>(size * size);
		}

		public override void UnInit()
		{
			base.UnInit();

			areas = null;
			free = null;
		}

		public override void Place(TrackSegment segment, int[,] spots)
		{
			base.Place(segment, spots);

			MapAreas(); //build list

			if(areas.Count == 0) //nothing to do, exit
				return;

			//determine actual possible pathes count - not all may fit
			int count = (areas.Count < targetCount)? areas.Count : targetCount;

			//place patches towards the target count
			for(int i = 0; i < count && areas.Count > 0; i++)
			{
				//choose area
				PlacerArea area = areas[Random.Range(0, areas.Count)];
				//builds free spots list
				area.GetSpots(PlacerSpot.Type.Empty, free);

				//choose from free spots and place assets
				for(int j = 0; j < assetsPerPatch; j++)
				{
					int index = Random.Range(0, free.Count);
					PlacerSpot spot = free[index];

					Mark(spot);
					AddAsset(spot);

					free.RemoveAt(index);
				}

				//prepare for next patch
				free.Clear();
				areas.RemoveAll(item => area.Overlaps(item) && (!allowIntertwined ||  allowIntertwined && item.GetSpotRatio(PlacerSpot.Type.Empty) < targetFillRatio));
			}

			//cleanup
			areas.Clear();
		}

		/// <summary>
		/// Scans segment's occupancy matrix and fills the areas list according to placer values.
		/// </summary>
		private void MapAreas()
		{
			PlacerSpot spot = new PlacerSpot();  //be gentle with the stack, allocate only once

			for(int i = 0; i <= TrackSegment.WIDTH - size; i++)
				for(int j = 0; j <= TrackSegment.LENGTH - size; j++)
				{
					spot.x = i;
					spot.y = j;

					float fill = PlacerArea.GetSpotRatio(PlacerSpot.Type.Empty, spots, spot, size);

					if(!allowIntertwined && !Mathf.Approximately(fill, 1f))
						continue;

					if(fill >= targetFillRatio)
						areas.Add(new PlacerArea(spots, spot, size));
				}
		}
	}
}