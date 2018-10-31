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
	/// Computes and places assets in a road like manner.
	/// To be used at first, because it overrides occupancy values.
	/// </summary>
	public class PathPlacer : Placer
	{
		/// <summary>
		/// On each build process we start from here.
		/// </summary>
		[SerializeField]
		[Range(0, TrackSegment.WIDTH)]
		private int startColumn = TrackSegment.WIDTH / 2;

		/// <summary>
		/// Assets no per row.
		/// </summary>
		[SerializeField]
		[Range(1, TrackSegment.WIDTH / 2)]
		private int thickness;

		/// <summary>
		/// Max horizontal offset.
		/// </summary>
		[SerializeField]
		[Range(1, TrackSegment.WIDTH / 2)]
		private int stretchWidthMax;

		/// <summary>
		/// Max vertical offset.
		/// </summary>
		[SerializeField]
		[Range(2, TrackSegment.LENGTH / 2)]
		private int stretchLengthMax;

		/// <summary>
		/// Current before each stretch.
		/// </summary>
		private int column;

		/// <summary>
		/// Column shift is retrieved from it.
		/// </summary>
		private PathStretch stretch;

		public override void Init()
		{
			base.Init();	

			column = startColumn;
			StartStretch();
		}

		public override void Place(TrackSegment segment, int[,] spots)
		{
			base.Place(segment, spots);

			PlacerSpot spot = new PlacerSpot();  //be gentle with the stack, allocate only once

			//iterate on each row
			for(int i = 0; i < TrackSegment.LENGTH; i++)
			{
				//get curent deviation
				int shift = stretch.Get();

				//mark and add assets along path's width
				for(int j = 0; j < thickness; j++ )
				{
					spot.x = column + shift + j;
					spot.y = i;

					Mark(spot);
					AddAsset(spot);
				}

				//increment stretch and start a new one if needed
				if(!stretch.Advance())
				{
					//save column for the next stretch
					column += shift; 
					StartStretch();
				}
			}
		}

		/// <summary>
		/// Determines random values for a new deviation.
		/// </summary>
		private void StartStretch()
		{
			int sign = Random.Range(0, 2) * 2 - 1; //value between -1 and 1

			int spread = Random.Range(0, stretchWidthMax);
			//make sure we don't go out of bounds
			if(column + sign * spread < 0) //check left
				spread = column;
			else if(column + sign * spread > TrackSegment.WIDTH - thickness) //check right
				spread = TrackSegment.WIDTH - 1 - column - thickness;

			int length = Random.Range(2, stretchLengthMax);

			//make sure that we have "smooth" interpolation
			if(length < spread)
				length = spread;
			
			stretch.Begin(spread, length, sign);
		}		
	}
}