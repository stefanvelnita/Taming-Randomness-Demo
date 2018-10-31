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

namespace Demo
{
	/// <summary>
	/// Computes incremental shift values.
	/// </summary>
	public struct PathStretch
	{
		/// <summary>
		/// Row distance to be covered.
		/// </summary>
		private int length;

		/// <summary>
		/// Direction: -1 = left, 1 = right.
		/// </summary>
		private int sign;

		/// <summary>
		/// Curent row index.
		/// </summary>
		private int row;

		/// <summary>
		/// Step value for moving towards target spread each row.
		/// </summary>
		private float gainPerRow;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="spread">Column distance to be covered.</param>
		/// <param name="length">Row distance to be covered.</param>
		/// <param name="sign">Direction: -1 = left, 1 = right.</param>
		public void Begin(int spread, int length, int sign)
		{
			this.length = length;
			this.sign = sign;
			row = 0;

			if(spread == 0)
				gainPerRow = 0f;
			else
				gainPerRow = (float)spread / length;
		}

		/// <summary>
		/// Returns column shift amount at current row.
		/// </summary>
		/// <returns></returns>
		public int Get()
		{
			//round to the nearest integer correctly
			return sign * (int)(row * gainPerRow + .5f);
		}

		/// <summary>
		/// Increment curent row towards target lengh.
		/// </summary>
		/// <returns>True if length reached, else false.</returns>
		public bool Advance()
		{
			return ++row != length;
		}
	}
}