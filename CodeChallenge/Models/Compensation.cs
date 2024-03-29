﻿using System;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        public String CompensationId { get; set; }
        public virtual Employee Employee { get; set; }
        public Double Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
