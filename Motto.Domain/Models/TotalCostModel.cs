﻿namespace Motto.Domain.Models
{
    /// <summary>
    /// Represents information about the total cost of a rental.
    /// </summary>
    public class TotalCostModel
    {
        /// <summary>
        /// Gets or sets the base cost of the rental.
        /// </summary>
        public decimal BaseCost { get; set; }

        /// <summary>
        /// Gets or sets the penalty cost of the rental.
        /// </summary>
        public decimal PenaltyCost { get; set; }

        /// <summary>
        /// Gets or sets the total cost of the rental.
        /// </summary>
        public decimal TotalCost { get; set; }
    }
}
