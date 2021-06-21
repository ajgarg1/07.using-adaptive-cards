using AdaptiveCards;
using AdaptiveCardsBot.Bots.Helper.BotAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaptiveCardsBot.Bots.Helper.AdaptiveCards
{
    public static class AdaptiveCardHelper
    {
        #region AddTextBlock
        public static AdaptiveTextBlock AddTextBlock(string value)
        {
            AdaptiveTextBlock newAdaptiveTextBlock = new AdaptiveTextBlock() { };
            newAdaptiveTextBlock.Text = value;
            return newAdaptiveTextBlock;
        }
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
        public static void AddTextBlockWithColumn(AdaptiveColumn column, string text, AdaptiveTextWeight TextWeight, AdaptiveTextSize size, AdaptiveTextColor TextColor, bool isSubTitle = true)
        {
            column.Items.Add(new AdaptiveTextBlock()
            {
                Text = text,
                Weight = TextWeight,
                Size = size,
                Color = TextColor,
                HorizontalAlignment = AdaptiveHorizontalAlignment.Center,
                IsSubtle = isSubTitle,
                Separation = AdaptiveSeparationStyle.None
            });
        }
        #endregion

        #region AddSubmitActionButton
        public static List<AdaptiveAction> AddButtonsToAdaptiveCard(OutputStack OutputStack)
        {
            var AdaptiveAction = new List<AdaptiveAction>();
            if (OutputStack.Data != null
                && OutputStack.Data._cognigy != null
                && OutputStack.Data._cognigy._default != null
                && OutputStack.Data._cognigy._default._buttons != null
                && OutputStack.Data._cognigy._default._buttons.buttons != null
                && OutputStack.Data._cognigy._default._buttons.buttons.Count() > 0)
            {
                int i = 1;
                foreach (var button in OutputStack.Data._cognigy._default._buttons.buttons)
                {
                    //string btnId = Guid.NewGuid().ToString();
                    string btnId = "btn";
                    btnId = btnId + Convert.ToString(i);
                    var dataJson = @"{""Type"":"" " + button.title + @" "",""Entered"":""test123""}";

                    var AdaptiveSubmitAction = AdaptiveCardHelper.AddButtonToAdaptiveCard(btnId, button, dataJson);
                    AdaptiveAction.Add(AdaptiveSubmitAction);
                    i = i + 1;
                }
                return AdaptiveAction;
            }

            return null;
        }
        public static AdaptiveSubmitAction AddButtonToAdaptiveCard(string btnId, Button btnObj, string dataJson)
        {
            //            return new AdaptiveSubmitAction() { Id = Convert.ToString(btnId), Title = btnActionText, DataJson = "{ \"Type\": \"ViewProfile\"}" };
            //https://weblog.west-wind.com/posts/2012/aug/30/using-jsonnet-for-dynamic-json-parsing
            //var jsonString = @"{""Name"":""Rick"",""Company"":""West Wind"",
            //            ""Entered"":""2012-03-16T00:03:33.245-10:00""}";

            //btnpayloadAction = "ViewProfile";
            //var DataJson = @"{""Type"":"" " + btnpActionCode + @" "",
            //            ""Entered"":""test123""}";

            // return new AdaptiveSubmitAction() { Id = btnId, Title = btnActionText, DataJson = "{ \"Type\": \"ViewProfile\"}" };

            return new AdaptiveSubmitAction()
            {
                Id = btnId,
                Title = btnObj.title,
                DataJson = dataJson,
                Type = "Action.Submit"
            };
        }
        #endregion

        #region AddDropDownList
        public static AdaptiveChoiceSetInput AddDropDownToAdaptiveCard(OutputStack OutputStack)
        {
            if (OutputStack.Data != null
              && OutputStack.Data._cognigy != null
              && OutputStack.Data._cognigy._default != null
              && OutputStack.Data._cognigy._default._buttons != null
              && OutputStack.Data._cognigy._default._buttons.buttons != null
              && OutputStack.Data._cognigy._default._buttons.buttons.Count() > 0)
            {
                return new AdaptiveChoiceSetInput()
                {
                    IsMultiSelect = false,
                    Choices = AddItemsToDropDown(OutputStack),
                    Style = AdaptiveChoiceInputStyle.Compact,
                    Id = "Category",
                    Value = OutputStack.Data._cognigy._default._buttons.buttons.First().title.ToString()
                };
            }
            return null;
        }
        public static List<AdaptiveChoice> AddItemsToDropDown(OutputStack OutputStack)
        {
            var AdaptiveChoices = new List<AdaptiveChoice>();
            foreach (var button in OutputStack.Data._cognigy._default._buttons.buttons)
            {
                var AdaptiveChoice = AdaptiveCardHelper.AddItemToDropDown(button);
                AdaptiveChoices.Add(AdaptiveChoice);
            }
            return AdaptiveChoices;
        }
        public static AdaptiveChoice AddItemToDropDown(Button choiceObj)
        {
            return new AdaptiveChoice()
            {
                Title = choiceObj.title,
                Value = choiceObj.title
            };
        }
        #endregion

        #region AddAdaptiveContainer
        public static AdaptiveContainer AddAdaptiveContainer(AdaptiveColumnSet columnSet)
        {
            var container = new AdaptiveContainer();
            container.Style = AdaptiveContainerStyle.Emphasis;
            container.Items.Add(columnSet);
            return container;
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
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
