namespace SLAPScheduling.Algorithm.TabuSearch
{
    public class Solution : IEquatable<Solution>
    {
        public List<int> Indices { get; private set; }

        #region Contructors
        public Solution()
        {
            Indices = new List<int>();
        }

        public Solution(List<int> indices)
        {
            Indices = indices;
        }

        #endregion

        #region Clone Method

        /// <summary>
        /// Creates a copy from the current Solution
        /// </summary>
        /// <returns></returns>
        public Solution Clone()
        {
            return new Solution(Indices.ToList());
        }

        #endregion

        #region Swap Solution
        /// <summary>
        /// Create a new solution by swapping two elements in the current solution
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public Solution? SwapSolution(int i, int j)
        {
            var clonedIndices = Indices.ToList();

            // Find the indices of the elements to swap
            int iIndex = clonedIndices.IndexOf(i);
            int jIndex = clonedIndices.IndexOf(j);

            if (iIndex != -1 && jIndex != -1)
            {
                // Perform the swap
                (clonedIndices[iIndex], clonedIndices[jIndex]) = (clonedIndices[jIndex], clonedIndices[iIndex]);
                return new Solution(clonedIndices);
            }

            return null;
        }

        #endregion

        #region Equality Comparer
        public bool Equals(Solution? other)
        {
            if (other == null)
            {
                return false;
            }

            return Indices.SequenceEqual(other.Indices);
        }

        public override int GetHashCode()
        {
            return Indices.GetHashCode();
        }

        #endregion
    }
}
