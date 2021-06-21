using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdaptiveCards;
using AdaptiveCardsBot.Bots.Helper.BotAPI;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;

namespace AdaptiveCardsBot.Bots.Helper.AdaptiveCards
{
    public class AdaptiveCardBotHelper
    {
        public static AdaptiveCard CreateHelloAdaptiveCard()
        {
            var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0));
            AdaptiveSubmitAction submitAction = new AdaptiveSubmitAction();
            submitAction.Title = "Hello";
            submitAction.Data = "hi";
            //var jsonString = @"{""Type"":""Details"",""Name"":""Rick"",""Company"":""West Wind"",
            //            ""Entered"":""2012-03-16T00:03:33.245-10:00""}";
            //submitAction.DataJson = jsonString;

            card.Actions.Add(submitAction);
            return card;
        }
        public static AdaptiveChoiceSetInput AddDropDownList()
        {
            var choices = new List<AdaptiveChoice>();
            choices.Add(new AdaptiveChoice()
            {
                Title = "Category 1",
                Value = "c1"
            });
            choices.Add(new AdaptiveChoice()
            {
                Title = "Category 2",
                Value = "c2"
            });

            AdaptiveChoiceSetInput choiceSet1 = new AdaptiveChoiceSetInput();
            choiceSet1.Choices = choices;

            var choiceSet = new AdaptiveChoiceSetInput()
            {
                IsMultiSelect = false,
                Choices = choices,
                Style = AdaptiveChoiceInputStyle.Compact,
                Id = "Category",
                Value = "c1"
            };
            return choiceSet;
        }
        public static AdaptiveCard GetCard(OutputStack OutputStack)
        {
            var mainCard = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0));
            if (OutputStack.Data != null
                && OutputStack.Data._cognigy != null
                && OutputStack.Data._cognigy._default != null
                && OutputStack.Data._cognigy._default._buttons != null)
            {
                string btnText = OutputStack.Data._cognigy._default._buttons.text;//to get buttons text
                mainCard.Body.Add(AdaptiveCardHelper.AddTextBlock(btnText));

                if (OutputStack.Data._cognigy._default._buttons.buttons != null && OutputStack.Data._cognigy._default._buttons.buttons.Count() > 0)
                //if (OutputStack.Data._cognigy._default._buttons.buttons != null && OutputStack.Data._cognigy._default._buttons.buttons.Count() > 0)
                {
                    int i = 1;
                    foreach (var button in OutputStack.Data._cognigy._default._buttons.buttons)
                    {
                        //string btnId = Guid.NewGuid().ToString();
                        string btnId = "btn";
                        btnId = btnId + Convert.ToString(i);
                        var dataJson = @"{""Type"":"" " + button.title + @" "",""Entered"":""test123""}";

                        mainCard.Actions.Add(AdaptiveCardHelper.AddButtonToAdaptiveCard(btnId, button, dataJson));
                        i = i + 1;
                    }
                    return mainCard;
                }
            }

            return null;
        }
        public static AdaptiveCard ViewProfile()
        {
            List<string> date = new List<string>() { "1-Jan", "26-Jan", "15-Aug" };
            List<string> des = new List<string>() { "New Year", "Republic Day", "Independence Day" };

            AdaptiveCard card = HolidayListAdaptiveCard(date, des);
            return card;
            //var reply = MessageFactory.Attachment(attachment);
            //await stepContext.Context.SendActivityAsync(reply);
            //return new DialogTurnResult(DialogTurnStatus.Waiting);
        }
        public static AdaptiveCard UpdateProfile()
        {

            var mainCard = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0));
            var column3 = new AdaptiveColumn();
            column3.Items.Add(new AdaptiveTextBlock() { Text = "Test Data", Weight = AdaptiveTextWeight.Bolder });
            //
            var column4 = new AdaptiveColumn();
            AdaptiveTextInput TextInput = new AdaptiveTextInput(); ;
            TextInput.Id = "GetUserInputKeywords";
            TextInput.Placeholder = "Please enter any vaue ";
            TextInput.MaxLength = 490;
            TextInput.IsMultiline = true;
            column4.Items.Add(TextInput);
            //

            var columnSet1 = new AdaptiveColumnSet();
            columnSet1.Columns.Add(column3);
            columnSet1.Columns.Add(column4);
            var container1 = new AdaptiveContainer();
            container1.Style = AdaptiveContainerStyle.Emphasis;
            container1.Items.Add(columnSet1);
            mainCard.Body.Add(container1);


            mainCard.Actions.Add(new AdaptiveSubmitAction() { Id = "ProfileUpdate", Title = "Submit", DataJson = "{ \"Type\": \"Update\" }" });

            return mainCard;
        }
        public static AdaptiveCard ReportIssue()
        {

            var mainCard = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0));
            var column3 = new AdaptiveColumn();
            column3.Items.Add(new AdaptiveTextBlock() { Text = "Test Data", Weight = AdaptiveTextWeight.Bolder });
            //
            var column4 = new AdaptiveColumn();
            AdaptiveTextInput TextInput = new AdaptiveTextInput(); ;
            TextInput.Id = "GetUserInput";
            TextInput.Placeholder = "Please enter any value ";
            TextInput.MaxLength = 490;
            TextInput.IsMultiline = true;
            column4.Items.Add(TextInput);
            //

            var columnSet1 = new AdaptiveColumnSet();
            columnSet1.Columns.Add(column3);
            columnSet1.Columns.Add(column4);
            var container1 = new AdaptiveContainer();
            container1.Style = AdaptiveContainerStyle.Emphasis;
            container1.Items.Add(columnSet1);
            mainCard.Body.Add(container1);


            mainCard.Actions.Add(new AdaptiveSubmitAction() { Id = "IssueButton", Title = "Submit", Data = "{ \"Type\": \"SendIssue\" }" });

            return mainCard;
        }
        public static AdaptiveCard HolidayListAdaptiveCard(List<string> date, List<string> description)
        {
            var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0));
            List<AdaptiveColumn> columns = new List<AdaptiveColumn>();
            AdaptiveColumn column = AdaptiveCardHelper.AddAdaptiveColumn();
            AdaptiveColumn column1 = AdaptiveCardHelper.AddAdaptiveColumn();
            AdaptiveColumn column2 = AdaptiveCardHelper.AddAdaptiveColumn();

            AdaptiveCardHelper.AddItemToAdaptiveColumn(column, AdaptiveCardHelper.AddTextBlock("Sr. No", AdaptiveTextWeight.Bolder, AdaptiveTextSize.Large, AdaptiveTextColor.Accent));
            AdaptiveCardHelper.AddItemToAdaptiveColumn(column1, AdaptiveCardHelper.AddTextBlock("Date", AdaptiveTextWeight.Bolder, AdaptiveTextSize.Large, AdaptiveTextColor.Good));
            AdaptiveCardHelper.AddItemToAdaptiveColumn(column2, AdaptiveCardHelper.AddTextBlock("Description", AdaptiveTextWeight.Bolder, AdaptiveTextSize.Large, AdaptiveTextColor.Dark));

            for (int i = 0; i < date.Count; i++)
            {
                AdaptiveCardHelper.AddItemToAdaptiveColumn(column, AdaptiveCardHelper.AddTextBlock((i + 1).ToString()));
                AdaptiveCardHelper.AddItemToAdaptiveColumn(column1, AdaptiveCardHelper.AddTextBlock((date[i]).ToString()));
                AdaptiveCardHelper.AddItemToAdaptiveColumn(column2, AdaptiveCardHelper.AddTextBlock((description[i]).ToString()));
            }

            var columnSet = new AdaptiveColumnSet();
            columnSet.Columns.Add(column);
            columnSet.Columns.Add(column1);
            columnSet.Columns.Add(column2);
            var container = new AdaptiveContainer();
            container.Style = AdaptiveContainerStyle.Emphasis;
            container.Items.Add(columnSet);
            card.Body.Add(container);
            return card;
        }
        public Attachment CardsDemoWithTypes()
        {
            string[] paths = { ".", "Resources", "AdaptiveCardWithButtons.json" };
            var cardJson = File.ReadAllText(Path.Combine(paths));
            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(cardJson),
            };
            return adaptiveCardAttachment;

            // caling
            //var adaptiveCardAttachment = CardsDemoWithTypes();
            //await turnContext.SendActivityAsync(MessageFactory.Attachment(adaptiveCardAttachment));
        }
    }

}
