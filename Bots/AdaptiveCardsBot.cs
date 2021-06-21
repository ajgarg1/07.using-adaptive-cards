using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveCards;
using AdaptiveCardsBot.Bots.Helper;
using AdaptiveCardsBot.Bots.Helper.AdaptiveCards;
using AdaptiveCardsBot.Bots.Helper.BotAPI;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.BotBuilderSamples
{

    /// <summary>
    /// Adaptive Cards are a way for developers to exchange card content
    /// </summary>
    public class AdaptiveCardsBot : ActivityHandler
    {

        private const string WelcomeText = @"Test.";
        private static RequestBody Body = null;
        BotAPIHelper apiHelperObj = null;

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            await SendWelcomeMessageAsync(turnContext, cancellationToken);
        }
        private static async Task SendWelcomeMessageAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in turnContext.Activity.MembersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    var userName = turnContext.Activity.From.Name;
                    await turnContext.SendActivityAsync(
                         //$"Welcome to Bot {member.Name}. {WelcomeText}",
                         //                         $"Welcome to Bot {member.Name}.",
                            $"Welcome to Bot {userName}.",
                         cancellationToken: cancellationToken);


                    //Create request body to sent it to connect api
                    Body = BotAPIService.CreateBody();

                    //////////////////////////////////
                    /////Add card on startup message

                    ////var attachments = new List<Attachment>();
                    ////var reply = MessageFactory.Attachment(attachments);
                    //Attachment cardAttachment = null;
                    ////if (text.Contains("testdc"))
                    ////{
                    //AdaptiveCard card = AdaptiveCardBotHelper.CreateHelloAdaptiveCard();
                    //cardAttachment = new Attachment()
                    //{
                    //    ContentType = AdaptiveCard.ContentType,
                    //    Content = card
                    //};
                    ////reply.Attachments.Add(cardAttachment);
                    ////}
                    //if (cardAttachment != null)
                    //{
                    //    await turnContext.SendActivityAsync(MessageFactory.Attachment(cardAttachment), cancellationToken);
                    //}
                }
            }
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            //var adaptiveCardAttachment = CardsDemoWithTypes();
            //await turnContext.SendActivityAsync(MessageFactory.Attachment(adaptiveCardAttachment));

            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                // Check if user submitted AdaptiveCard input
                if (turnContext.Activity.Value != null)
                {
                    //var activityValue = turnContext.Activity.AsMessageActivity().Value as Newtonsoft.Json.Linq.JObject;
                    //if (activityValue != null)
                    //{
                    //    var categorySelection = activityValue.ToObject<CategorySelection>();
                    //    var category = categorySelection.Category;
                    //    await turnContext.SendActivityAsync(category);
                    //}

                        // Convert String to JObject
                        String value = turnContext.Activity.Value.ToString();
                    JObject results = JObject.Parse(value);

                    // Get type from input field
                    String submitType = results.GetValue("Type").ToString().Trim();
                    switch (submitType)
                    {
                        case "email":
                            IMessageActivity message = Activity.CreateMessageActivity();
                            message.Type = ActivityTypes.Message;
                            message.Text = "email \n <br> sent";
                            message.Locale = "en-Us";
                            message.TextFormat = TextFormatTypes.Plain;
                            await turnContext.SendActivityAsync(message,cancellationToken);
                            /* */
                            return;
                        default:
                            await turnContext.SendActivityAsync("No Action Logic Written for this button", cancellationToken: cancellationToken);
                            break;
                    }

                    String name = results.GetValue("Type").ToString().Trim();
                    //String actionText = results.GetValue("ActionText").ToString();
                    //await turnContext.SendActivityAsync("Respond to user " + actionText, cancellationToken: cancellationToken);

                    // Get Keywords from input field
                    String userInputKeywords = "";
                    //                    if (name == "GetPPT") {
                    if (name == "ViewProfile")
                    {
                        //String DisplayVal = results.GetValue("DisplayText").ToString();
                        //await turnContext.SendActivityAsync(MessageFactory.Text(DisplayVal), cancellationToken);

                        userInputKeywords = "View Profile";

                        AdaptiveCard ViewcardAttachment = null;
                        ViewcardAttachment = AdaptiveCardBotHelper.ViewProfile();

                        var attachment = new Attachment
                        {
                            ContentType = AdaptiveCard.ContentType,
                            Content = ViewcardAttachment
                        };

                        if (attachment != null)
                        {
                            await turnContext.SendActivityAsync(MessageFactory.Attachment(attachment), cancellationToken);
                            //await turnContext.SendActivityAsync(MessageFactory.Text("Please enter any text to see another card."), cancellationToken);
                        }

                    }
                    else if (name == "UpdateProfile")
                    {
                        userInputKeywords = "Update Profile";

                        AdaptiveCard UpdatecardAttachment = null;
                        UpdatecardAttachment = AdaptiveCardBotHelper.UpdateProfile();

                        var attachment = new Attachment
                        {
                            ContentType = AdaptiveCard.ContentType,
                            Content = UpdatecardAttachment
                        };

                        if (attachment != null)
                        {
                            await turnContext.SendActivityAsync(MessageFactory.Attachment(attachment), cancellationToken);
                            //await turnContext.SendActivityAsync(MessageFactory.Text("Please enter any text to see another card."), cancellationToken);
                        }

                        //userInputKeywords = results.GetValue("GetUserInputKeywords").ToString();
                    }
                    else if (name == "SendIssue")
                    {
                        AdaptiveCard IssuecardAttachment = null;
                        IssuecardAttachment = AdaptiveCardBotHelper.ReportIssue();

                        var attachment = new Attachment
                        {
                            ContentType = AdaptiveCard.ContentType,
                            Content = IssuecardAttachment
                        };

                        if (attachment != null)
                        {
                            await turnContext.SendActivityAsync(MessageFactory.Attachment(attachment), cancellationToken);
                            //await turnContext.SendActivityAsync(MessageFactory.Text("Please enter any text to see another card."), cancellationToken);
                        }

                        userInputKeywords = "Report Issue";
                    }
                    else if (name == "Update")
                    {
                        userInputKeywords = "Update Info";
                        userInputKeywords = results.GetValue("GetUserInputKeywords").ToString();
                    }
                    //

                    // Make Http request to api with paramaters
                    //String myUrl = $"http://myurl.com/api/{userInputKeywords}";

                    //...

                    // Respond to user
                    await turnContext.SendActivityAsync("Respond to user" + userInputKeywords, cancellationToken: cancellationToken);
                }
                else
                {
                    //Conversation Text:- hi, "megha.gupta@rsystems.com", "i want to raise an issue", "hardware", "software"
                    turnContext.Activity.RemoveRecipientMention();
                    var text = turnContext.Activity.Text.Trim().ToLower();
                    Body.Text = text;
                    apiHelperObj = new BotAPIHelper();
                    CreateResponseBody responseBody = apiHelperObj.CreateApiPostCall(Body);
                    if (responseBody != null)
                    {
                        if (responseBody.OutputStack != null && responseBody.OutputStack.Count() > 0)
                        {
                            foreach (var OutputStack in responseBody.OutputStack)
                            {
                                if (!string.IsNullOrEmpty(OutputStack.Text))
                                {
                                    await turnContext.SendActivityAsync(MessageFactory.Text(OutputStack.Text), cancellationToken);

                                    //IMessageActivity message = Activity.CreateMessageActivity();
                                    //message.Type = ActivityTypes.Message;
                                    //message.Text = "your \n <br> text";
                                    //message.Locale = "en-Us";
                                    //message.TextFormat = TextFormatTypes.Plain;
                                    //await turnContext.SendActivityAsync(message);
                                }
                                else
                                {
                                    var mainCard = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0));
                                    if (OutputStack.Data.type == "buttons")
                                    {
                                        //===========================Add DropDownList
                                        //AdaptiveChoiceSetInput choiceSet = AdaptiveCardBotHelper.AddDropDownList();
                                        AdaptiveChoiceSetInput choiceSet = AdaptiveCardHelper.AddDropDownToAdaptiveCard(OutputStack);
                                        mainCard.Body.Add(choiceSet);

                                        //=========================== Add Button
                                        var adaptiveButtonList = AdaptiveCardHelper.AddButtonsToAdaptiveCard(OutputStack);
                                        if (adaptiveButtonList != null && adaptiveButtonList.Count() > 0)
                                        {
                                            mainCard.Body.Add(AdaptiveCardHelper.AddTextBlock(OutputStack.Data._cognigy._default._buttons.text));
                                            mainCard.Actions = adaptiveButtonList;
                                        }
                                        //var card = AdaptiveCardBotHelper.GetCard(OutputStack);
                                    }
                                    if (mainCard != null)
                                    {
                                        var cardAttachment = new Attachment()
                                        {
                                            ContentType = AdaptiveCard.ContentType,
                                            Content = mainCard,
                                            Name = "CardName"
                                        };
                                        await turnContext.SendActivityAsync(MessageFactory.Attachment(cardAttachment), cancellationToken);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}






