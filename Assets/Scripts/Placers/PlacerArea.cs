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

namespace Demo
{
	/// <summary>
	/// Defines an region in a segment's occupancy matrix.
	/// </summary>
	public class PlacerArea
	{
		/// <summary>
		/// Segment's occupancy matrix ref.
		/// </summary>
		public int[,] spots;
		/// <summary>
		/// Bottom left corner.
		/// </summary>
		public PlacerSpot pivot;
		/// <summary>
		/// Extents of the area on x and y axis.
		/// </summary>
		public int size;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="spots">Ref to occupancy matrix.</param>
		/// <param name="pivot">Start of area.</param>
		/// <param name="size">Extents of area.</param>
		public PlacerArea(int[,] spots, PlacerSpot pivot, int size)
		{
			this.spots = spots;
			this.pivot = pivot;
			this.size = size;
		}

		/// <summary>
		/// Check to see if it intersects another area.
		/// </summary>
		/// <param name="area">Other area.</param>
		/// <returns>True, if areas intersect, else false</returns>
		public bool Overlaps(PlacerArea area)
		{
			return pivot.x >= area.pivot.x - (size - 1)
				&& pivot.x <= area.pivot.x + area.size - 1
				&& pivot.y >= area.pivot.y - (size - 1)
				&& pivot.y <= area.pivot.y + area.size - 1;
		}

		/// <summary>
		/// Scans the surface of the occupancy matrix for a specific spot type and computes it's ratio.
		/// </summary>
		/// <param name="type">Value to search for.</param>
		/// <returns>A value between [0f, 1f].</returns>
		public float GetSpotRatio(PlacerSpot.Type type)
		{
			return GetSpotRatio(type, spots, pivot, size);
		}

		/// <summary>
		/// Scans the surface of a segment's occupancy matrix for a specific spot type and computes it's ratio.
		/// </summary>
		/// <param name="type">Value to search for.</param>
		/// <param name="spots">Ref to occupancy matrix.</param>
		/// <param name="pivot">Start of area.</param>
		/// <param name="size">Extents of area.</param>
		/// <returns>A value between [0f, 1f].</returns>
		public static float GetSpotRatio(PlacerSpot.Type type, int[,] spots, PlacerSpot pivot, int size)
		{
			int count = 0;

			for(int i = 0; i < size; i++)
				for(int j = 0; j < size; j++)
					if(spots[pivot.x + i, pivot.y + j] == (int)type)
						count++;
			
			return (float)count / (size * size);		
		}

		/// <summary>
		/// Returns all the spots of a certain type.
		/// </summary>
		/// <param name="type">Value to search for.</param>
		/// <param name="output">List to add the spots to.</param>
		public void GetSpots(PlacerSpot.Type type, List<PlacerSpot> output)
		{
			GetSpots(type, spots, pivot, size, output);
		}

		/// <summary>
		/// Returns all the spots of a certain type from a specified area.
		/// </summary>
		/// <param name="type">Value to search for.</param>
		/// <param name="spots">Ref to occupancy matrix.</param>
		/// <param name="pivot">Start of area.</param>
		/// <param name="size">Extents of area.</param>
		/// <param name="output">List to add the spots to.</param>
		public static void GetSpots(PlacerSpot.Type type, int[,] spots, PlacerSpot pivot, int size, List<PlacerSpot> output)
		{
			PlacerSpot spot = new PlacerSpot();  //be gentle with the stack, allocate only once

			for(int i = 0; i < size; i++)
				for(int j = 0; j < size; j++)
				{
					spot.x = pivot.x + i;
					spot.y = pivot.y + j;

					if(spots[spot.x, spot.y] == (int)type)
						output.Add(spot);
				}
		}
	}
}