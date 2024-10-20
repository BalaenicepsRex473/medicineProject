﻿using Microsoft.Build.Framework;

namespace scrubsAPI
{
    public class PatientCreateModel
    {
        [Required] public string name { get; set; }
        public DateTime? birthday { get; set; }
        [Required] public Sex gender { get; set; }
    }
}
