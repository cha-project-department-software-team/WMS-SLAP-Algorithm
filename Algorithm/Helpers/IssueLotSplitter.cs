namespace SLAPScheduling.Algorithm.Helpers
{
    public class IssueLotSplitter
    {
        private List<IssueLot> issueLots { get; set; }

        #region Constructor
        public IssueLotSplitter(List<IssueLot> issueLots)
        {
            this.issueLots = issueLots;
        }

        #endregion

        #region Split Issue Lots to multiple Issue Sublots

        /// <summary>
        /// Create Issue Sublots from Issue Lot and Material Lot
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IssueSublot> GetIssueSubLots()
        {
            foreach (var issueLot in this.issueLots)
            {
                var materialLot = issueLot.materialLot;
                var requestedQuantity = issueLot.requestedQuantity;

                if (materialLot is not null && materialLot.subLots?.Count > 0 && materialLot.exisitingQuantity >= requestedQuantity)
                {
                    var materialSublots = materialLot.subLots.OrderBy(x => x.location.GetDistanceToIOPoint()).ToList();
                    for (int index = 0; index < materialSublots.Count; index++)
                    {
                        var materialSublot = materialSublots[index];

                        var sublotId = $"{issueLot.issueLotId}_{index}";
                        var sublotQuantity = materialSublot.existingQuality;
                        var issueSublotQuantity = sublotQuantity <= requestedQuantity ? sublotQuantity : requestedQuantity;
                        if (issueSublotQuantity > 0)
                        {
                            requestedQuantity -= issueSublotQuantity;
                            var issueSublot = new IssueSublot(issueSublotId: sublotId,
                                                              requestedQuantity: issueSublotQuantity,
                                                              materialSublotId: materialSublot.subLotId,
                                                              issueLotId: issueLot.issueLotId);

                            issueSublot.UpdateMaterialSublot(materialSublot);
                            issueSublot.UpdateIssueLot(issueLot);

                            yield return issueSublot;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
