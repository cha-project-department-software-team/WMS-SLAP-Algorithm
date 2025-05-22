using SLAPScheduling.Domain.AggregateModels.MaterialAggregate.MaterialLots;

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
        public List<IssueSublot> GetIssueSubLots()
        {
            var issueSubLots = new List<IssueSublot>();
            foreach (var issueLot in this.issueLots)
            {
                var materialLot = issueLot.materialLot;
                var requestedQuantity = issueLot.requestedQuantity;

                if (materialLot is not null && materialLot.subLots?.Count > 0 && CheckAvailableQuantity(materialLot, issueLot))
                {
                    var materialSublots = materialLot.subLots.Where(x => x.existingQuality > 0).OrderBy(x => x.location.GetDistanceToIOPoint()).ToList();
                    for (int index = 0; index < materialSublots.Count; index++)
                    {
                        var materialSublot = materialSublots[index];
                        var sublotQuantity = GetAvailableSubLotQuantity(materialSublot, issueSubLots);

                        var issueSublotQuantity = sublotQuantity <= requestedQuantity ? sublotQuantity : requestedQuantity;
                        if (issueSublotQuantity > 0)
                        {
                            requestedQuantity -= issueSublotQuantity;
                            var issueSublot = new IssueSublot(issueSublotId: $"{issueLot.issueLotId}_{index}",
                                                              requestedQuantity: issueSublotQuantity,
                                                              materialSublotId: materialSublot.subLotId,
                                                              issueLotId: issueLot.issueLotId);

                            issueSublot.UpdateMaterialSublot(materialSublot);
                            issueSublot.UpdateIssueLot(issueLot);

                            issueSubLots.Add(issueSublot);
                        }
                    }
                }
            }

            return issueSubLots;
        }

        private bool CheckAvailableQuantity(MaterialLot materialLot, IssueLot issueLot)
        {
            var storingQuantity = materialLot.exisitingQuantity;

            var lotNumber = materialLot.lotNumber;
            var otherRequestQuantity = this.issueLots.Where(x => !x.Equals(issueLot) && x.materialLot.lotNumber.Equals(lotNumber)).Sum(x => x.requestedQuantity);
            
            var availableQuantity = storingQuantity - otherRequestQuantity;
            return availableQuantity >= issueLot.requestedQuantity;
        }

        private double GetAvailableSubLotQuantity(MaterialSubLot materialSubLot, List<IssueSublot> issueSublots)
        {
            var storingQuantity = materialSubLot.existingQuality;
            var requestedQuantity = issueSublots.Where(x => x.sublotId.Equals(materialSubLot.subLotId)).Sum(x => x.requestedQuantity);

            return storingQuantity - requestedQuantity;
        }

        #endregion
    }
}
