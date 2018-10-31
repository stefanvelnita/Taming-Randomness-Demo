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
	/// Base class to be derived from by all placers.
	/// </summary>
	public abstract class Placer : MonoBehaviour
	{
		/// <summary>
		/// Added to segment's occupancy matrix.
		/// Besides showing that a spot is filled, some placerers might place only around or on top of certain spots.
		/// </summary>
		[SerializeField]
		protected PlacerSpot.Type mark;

		/// <summary>
		/// Object to be instantiated and placed on segment.
		/// </summary>
		[SerializeField]
		protected GameObject asset;

		/// <summary>
		/// Random position offset limits which will be applied to the asset.
		/// </summary>
		[SerializeField]
		private Vector3 positionsRange;

		/// <summary>
		/// Random rotation offset limits which will be applied to the asset.
		/// </summary>
		[SerializeField]
		private Vector3 rotationsRange;

		/// <summary>
		/// Current processed segment.
		/// </summary>
		protected TrackSegment segment;

		/// <summary>
		/// Current segment's occupancy status.
		/// </summary>
		protected int[,] spots;

		/// <summary>
		/// Called before build process started.
		/// All allocations go in here.
		/// </summary>
		public virtual void Init()
		{		
		}

		/// <summary>
		/// Called after build process ended.
		/// All deallocations go in here.
		/// </summary>
		public virtual void UnInit()
		{
			segment = null;
			spots = null;
		}

		/// <summary>
		/// Called for each segment. Asset placement logic goes in here.
		/// </summary>
		/// <param name="segment">Current processed segment.</param>
		/// <param name="spots">Occupancy status of the segment.</param>
		public virtual void Place(TrackSegment segment, int[,] spots)
		{
			this.segment = segment;
			this.spots = spots;
		}

		/// <summary>
		/// Set mark value in occupation matrix.
		/// </summary>
		/// <param name="spot">Matrix coordinates.</param>
		protected void Mark(PlacerSpot spot)
		{
			spots[spot.x, spot.y] = (int)mark;
		}

		/// <summary>
		/// Instatiates a clone of the asset and positions it in segment's space, applying the offsets.
		/// </summary>
		/// <param name="spot">Matrix coordinates.</param>
		protected void AddAsset(PlacerSpot spot)
		{
			//determine offsets
			Vector3 posOffset = new Vector3(
				Random.Range(-positionsRange.x, positionsRange.x),
				Random.Range(-positionsRange.y, positionsRange.y),
				Random.Range(-positionsRange.z, positionsRange.z)
			);

			Vector3 rotOffset = new Vector3(
				Random.Range(-rotationsRange.x, rotationsRange.x),
				Random.Range(-rotationsRange.y, rotationsRange.y),
				Random.Range(-rotationsRange.z, rotationsRange.z)
			);
			
			//create and place clone
			Instantiate(asset, segment.GetSpotPos(spot.x, spot.y) + posOffset, Quaternion.Euler(rotOffset), segment.transform);
		}
	}
}