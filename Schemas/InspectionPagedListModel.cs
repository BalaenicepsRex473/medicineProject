﻿namespace scrubsAPI
{
    public class InspectionPagedListModel
    {
        public List<InspectionPreviewModel> inspections { get; set; }
        public PageInfoModel pagination { get; set; }
    }
}
