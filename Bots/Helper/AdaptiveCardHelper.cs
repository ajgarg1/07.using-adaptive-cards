using AdaptiveCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaptiveCardsBot.Bots.Helper
{
    public static class AdaptiveCardHelper
    {
        public static AdaptiveTextBlock AddTextBlock(string value, AdaptiveTextWeight TextWeight, AdaptiveTextSize TextSize, AdaptiveTextColor TextColor)
        {
            AdaptiveTextBlock newAdaptiveTextBlock = new AdaptiveTextBlock() { };
            newAdaptiveTextBlock.Text = value;

            //newAdaptiveTextBlock.Text = "Sr. No";
            newAdaptiveTextBlock.Weight = TextWeight;
            newAdaptiveTextBlock.Size = TextSize;
            newAdaptiveTextBlock.Color = TextColor;
            return newAdaptiveTextBlock;
        }
        public static AdaptiveTextBlock AddTextBlock(string value)
        {
            AdaptiveTextBlock newAdaptiveTextBlock = new AdaptiveTextBlock() { };
            newAdaptiveTextBlock.Text = value;

            //newAdaptiveTextBlock.Text = "Sr. No";
            newAdaptiveTextBlock.Weight = AdaptiveTextWeight.Bolder;
            newAdaptiveTextBlock.Size = AdaptiveTextSize.Large;
            newAdaptiveTextBlock.Color = AdaptiveTextColor.Accent;
            return newAdaptiveTextBlock;
        }
        public static AdaptiveColumn AddAdaptiveColumn()
        {
            return new AdaptiveColumn();
        }
        public static void AddItemToAdaptiveColumn(AdaptiveColumn column, AdaptiveTextBlock value)
        {
            column.Items.Add(value);
        }
        public static void AddItemToAdaptiveColumn(AdaptiveColumn column, AdaptiveElement value)
        {
            column.Items.Add(value);
        }

        //public static AdaptiveColumn AddAdaptiveColumn(AdaptiveElement value)
        //{
        //    AdaptiveColumn column = new AdaptiveColumn();
        //    column.Items.Add(value);
        //    return column;
        //}
        //public static AdaptiveColumn AddAdaptiveColumn(AdaptiveTextBlock value)
        //{
        //    AdaptiveColumn column = new AdaptiveColumn();
        //    column.Items.Add(value);
        //    return column;
        //}
    }
}
