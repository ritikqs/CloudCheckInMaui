using System;
using System.Collections.Generic;
using Microsoft.Maui.Graphics;

namespace CloudCheckInMaui.Models
{
    // This is a simplified version without calendar plugin - will need implementation when adding functionality
    public class DayEventCollection<T> : List<T>
    {
        /// <summary>
        /// Empty constructor extends from base()
        /// </summary>
        public DayEventCollection() : base()
        {
        }

        /// <summary>
        /// Color constructor extends from base()
        /// </summary>
        /// <param name="eventIndicatorColor"></param>
        /// <param name="eventIndicatorSelectedColor"></param>
        public DayEventCollection(Color eventIndicatorColor, Color eventIndicatorSelectedColor) : base()
        {
            EventIndicatorColor = eventIndicatorColor;
            EventIndicatorSelectedColor = eventIndicatorSelectedColor;
        }

        /// <summary>
        /// IEnumerable constructor extends from base(IEnumerable collection)
        /// </summary>
        /// <param name="collection"></param>
        public DayEventCollection(IEnumerable<T> collection) : base(collection)
        {
        }

        /// <summary>
        /// Capacity constructor extends from base(int capacity)
        /// </summary>
        /// <param name="capacity"></param>
        public DayEventCollection(int capacity) : base(capacity)
        {
        }

        #region Properties
        public Color EventIndicatorColor { get; set; }
        public Color EventIndicatorSelectedColor { get; set; }
        public Color EventIndicatorTextColor { get; set; }
        public Color EventIndicatorSelectedTextColor { get; set; }
        #endregion
    }

    public class AdvancedEventModel
    {
        public string Description { get; set; }
        public string ApprovedBy { get; set; }
    }
} 