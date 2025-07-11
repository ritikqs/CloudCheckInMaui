#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Graphics;

namespace CCIMIGRATION.Models
{
    // Simple interface to replace IPersonalizableDayEvent
    public interface IEventModel
    {
        Color? EventIndicatorColor { get; set; }
        Color? EventIndicatorSelectedColor { get; set; }
        Color? EventIndicatorTextColor { get; set; }
        Color? EventIndicatorSelectedTextColor { get; set; }
    }

    public class DayEventCollection<T> : List<T>, IEventModel
    {
        /// <summary>
        /// Empty contructor extends from base()
        /// </summary>
        public DayEventCollection() : base()
        {

        }

        /// <summary>
        /// Color contructor extends from base()
        /// </summary>
        /// <param name="eventIndicatorColor"></param>
        /// <param name="eventIndicatorSelectedColor"></param>
        public DayEventCollection(Color? eventIndicatorColor, Color? eventIndicatorSelectedColor) : base()
        {
            EventIndicatorColor = eventIndicatorColor;
            EventIndicatorSelectedColor = eventIndicatorSelectedColor;
        }

        /// <summary>
        /// IEnumerable contructor extends from base(IEnumerable collection)
        /// </summary>
        /// <param name="collection"></param>
        public DayEventCollection(IEnumerable<T> collection) : base(collection)
        {

        }

        /// <summary>
        /// Capacity contructor extends from base(int capacity)
        /// </summary>
        /// <param name="capacity"></param>
        public DayEventCollection(int capacity) : base(capacity)
        {

        }

        #region PersonalizableProperties
        public Color? EventIndicatorColor { get; set; }
        public Color? EventIndicatorSelectedColor { get; set; }
        public Color? EventIndicatorTextColor { get; set; }
        public Color? EventIndicatorSelectedTextColor { get; set; }

        #endregion

    }
    public class AdvancedEventModel
    {
        public string Description { get; set; } = string.Empty;
        public string ApprovedBy { get; set; } = string.Empty;
    }
}
