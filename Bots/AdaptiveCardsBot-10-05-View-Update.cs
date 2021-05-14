// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using AdaptiveCards;
using AdaptiveCardsBot.Bots.Helper;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.BotBuilderSamples
{
    // This bot will respond to the user's input with an Adaptive Card.
    // Adaptive Cards are a way for developers to exchange card content
    // in a common and consistent way. A simple open card format enables
    // an ecosystem of shared tooling, seamless integration between apps,
    // and native cross-platform performance on any device.
    // For each user interaction, an instance of this class is created and the OnTurnAsync method is called.
    // This is a Transient lifetime service. Transient lifetime services are created
    // each time they're requested. For each Activity received, a new instance of this
    // class is created. Objects that are expensive to construct, or have a lifetime
    // beyond the single turn, should be carefully managed.

    public class AdaptiveCardsBot2 : ActivityHandler
    {
        private string _appId = "test";
        private string _appPassword = "test1";

        //public AdaptiveCardsBot(IConfiguration config)
        //{
        //    _appId = config["MicrosoftAppId"];
        //    _appPassword = config["MicrosoftAppPassword"];
        //}

        private const string WelcomeText = @"Test.";

        // This array contains the file location of our adaptive cards
        private readonly string[] _cards =
        {
            Path.Combine(".", "Resources", "FlightItineraryCard.json"),
            Path.Combine(".", "Resources", "ImageGalleryCard.json"),
            Path.Combine(".", "Resources", "LargeWeatherCard.json"),
            Path.Combine(".", "Resources", "RestaurantCard.json"),
            Path.Combine(".", "Resources", "SolitaireCard.json"),
        };

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            await SendWelcomeMessageAsync(turnContext, cancellationToken);
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                // Check if user submitted AdaptiveCard input
                if (turnContext.Activity.Value != null) {

                    // Convert String to JObject
                    String value = turnContext.Activity.Value.ToString();
                    JObject results = JObject.Parse(value);

                    // Get type from input field
                    String name = results.GetValue("Type").ToString();
                    //String actionText = results.GetValue("ActionText").ToString();
                    //await turnContext.SendActivityAsync("Respond to user " + actionText, cancellationToken: cancellationToken);

                    // Get Keywords from input field
                    String userInputKeywords = "";
//                    if (name == "GetPPT") {
                  if(name == "ViewProfile")
                    {
                        //String DisplayVal = results.GetValue("DisplayText").ToString();
                        //await turnContext.SendActivityAsync(MessageFactory.Text(DisplayVal), cancellationToken);

                        userInputKeywords = "View Profile";

                        AdaptiveCard ViewcardAttachment = null;
                        ViewcardAttachment = ViewProfile();

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
                        UpdatecardAttachment = UpdateProfile();

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
                        IssuecardAttachment = ReportIssue();

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
                    //// Send user AdaptiveCard
                    ////var cardAttachment = GetUserInputForCustomPPT();
                    ////var reply = turnContext.Activity.CreateReply();
                    ////reply.Attachments = new List<Attachment>() { cardAttachment };
                    ////await turnContext.SendActivityAsync(reply, cancellationToken);

                    //var cardAttachment = GetUserInputForCustomPPT();
                    ////await turnContext.SendActivityAsync(MessageFactory.Attachment(cardAttachment), cancellationToken);
                    //await turnContext.SendActivityAsync(MessageFactory.Attachment(cardAttachment), cancellationToken);
                    //await turnContext.SendActivityAsync(MessageFactory.Text("Please enter any text to see another card."), cancellationToken);

                    //2.
                    turnContext.Activity.RemoveRecipientMention();
                    var text = turnContext.Activity.Text.Trim().ToLower();
                    var attachments = new List<Attachment>();
                    var reply = MessageFactory.Attachment(attachments);
                    Attachment cardAttachment = null;
                    //if (text.Contains("testdc"))
                    if (text.Contains("hello"))
                    {
                        //AdaptiveCard card = CreateAdaptiveCard_WithDynamicContent1();
                        AdaptiveCard card = DisplayActionCards();
                        cardAttachment = new Attachment()
                        {
                            ContentType = AdaptiveCard.ContentType,
                            Content = card
                        };
                        reply.Attachments.Add(cardAttachment);
                    }
                    if (cardAttachment != null)
                    {
                        await turnContext.SendActivityAsync(MessageFactory.Attachment(cardAttachment), cancellationToken);
                        //await turnContext.SendActivityAsync(MessageFactory.Text("Please enter any text to see another card."), cancellationToken);
                    }
                }
            }
        }
        public static AdaptiveCard DisplayActionCards()
        {
            var mainCard = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0));

            //for (int i = 0; i < city.Count; i++)
            //{
            //mainCard.Actions.Add(new AdaptiveShowCardAction() { Title = city[i], Card = cards[i] });
            //mainCard.Actions.Add(new AdaptiveSubmitAction() {Title="Submit",Data= "Submit" });
            //mainCard.Actions.Add(new AdaptiveSubmitAction() { Id = "contactSubmit", Title = "Submit", Data = "{ \"Type\": \"GetPPT\" }" });
            mainCard.Actions.Add(new AdaptiveSubmitAction() { Id = "ViewProfile", Title = "View Profile", DataJson = "{ \"Type\": \"ViewProfile\"}" });
            mainCard.Actions.Add(new AdaptiveSubmitAction() { Id = "UpdateProfile", Title = "Update Profile", DataJson = "{ \"Type\": \"UpdateProfile\" }" });
            mainCard.Actions.Add(new AdaptiveSubmitAction() { Id = "ReportIssue", Title = "Report Issue", DataJson = "{ \"Type\": \"ReportIssue\" }" });

            //{ \"Type\": \"GetPPTNo\" }
            //mainCard.Actions.Add(new AdaptiveOpenUrlAction() { Title = "OpenUrl", Url = new Uri("https://adaptivecards.io/explorer/Action.OpenUrl.html") });
            //}

            var attachment = new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = mainCard
            };

            return mainCard;
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
        public static AdaptiveCard CreateAdaptiveCard_WithDynamicContent1()
        {
            List<string> city = new List<string>() { "Delhi", "Bangalore", "Mumbai" };
            List<string> date = new List<string>() { "1-Jan", "26-Jan", "15-Aug" };
            List<string> des = new List<string>() { "New Year", "Republic Day", "Independence Day" };

            List<string> date1 = new List<string>() { "1-Jan", "26-Jan", "15-Aug", "25-Dec" };
            List<string> des1 = new List<string>() { "New Year", "Republic Day", "Independence Day", "Christmas Day" };

            List<string> date2 = new List<string>() { "1-Jan", "25-Dec" };
            List<string> des2 = new List<string>() { "New Year", "Christmas Day" };

            List<AdaptiveCard> cards = new List<AdaptiveCard>();
            cards.Add(HolidayListAdaptiveCard(date, des));
            cards.Add(HolidayListAdaptiveCard(date1, des1));
            cards.Add(HolidayListAdaptiveCard(date2, des2));

            var mainCard = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0));
            var column3 = new AdaptiveColumn();
            column3.Items.Add(new AdaptiveTextBlock() { Text = "Holiday City", Weight = AdaptiveTextWeight.Bolder });
            //
            var column4 = new AdaptiveColumn();
            AdaptiveTextInput TextInput =  new AdaptiveTextInput();;
            TextInput.Id = "GetUserInputKeywords";
            TextInput.Placeholder = "Please enter the keyword list separated by ',' Ex:RPA,FS ";
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

            List<AdaptiveShowCardAction> adaptiveShowCardActions = new List<AdaptiveShowCardAction>();
            //for (int i = 0; i < city.Count; i++)
            //{
            //mainCard.Actions.Add(new AdaptiveShowCardAction() { Title = city[i], Card = cards[i] });
            //mainCard.Actions.Add(new AdaptiveSubmitAction() {Title="Submit",Data= "Submit" });
            mainCard.Actions.Add(new AdaptiveSubmitAction() { Id = "contactSubmit", Title = "Submit", Data = "{ \"Type\": \"GetPPT\" }" });
            //{ \"Type\": \"GetPPTNo\" }
            //mainCard.Actions.Add(new AdaptiveOpenUrlAction() { Title = "OpenUrl", Url = new Uri("https://adaptivecards.io/explorer/Action.OpenUrl.html") });
            //}

            var attachment = new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = mainCard
            };

            return mainCard;
            //var reply = MessageFactory.Attachment(attachment);
            //await stepContext.Context.SendActivityAsync(reply);
            //return new DialogTurnResult(DialogTurnStatus.Waiting);
        }
        public static Attachment GetUserInputForCustomPPT()
        {
            AdaptiveCard card = new AdaptiveCard();
            card.Body.Add(new AdaptiveTextBlock()
            {
                Text = "Do you want to apply filter and customise the PPT?",
                Wrap = true,
                Size = AdaptiveTextSize.Small
            });

            card.Body.Add(new AdaptiveContainer()
            {
                Id = "getCustomPPTNo",
                SelectAction = new AdaptiveSubmitAction()
                {
                    Id = "getCustomPPTNo",
                    Title = "No",
                    DataJson = "{ \"Type\": \"GetCustomPPT\" }",
                }
            });

            card.Body.Add(new AdaptiveContainer()
            {
                Id = "getCustomPPTYes",
                Items = new List<AdaptiveElement>()
                    {
                        new AdaptiveTextBlock()
                        {
                            Text = "Please select an option",
                            Wrap=true,
                            Size = AdaptiveTextSize.Small
                        }
                    }
            });


            card.Actions.Add(new AdaptiveShowCardAction()
            {
                Id = "GetPPTYes",
                Title = "Yes",
                Card = new AdaptiveCard()
                {
                    Body = new List<AdaptiveElement>()
                        {
                            new AdaptiveTextBlock()
                            {
                                Text = "Please enter your input",
                                Wrap = true
                            },
                            new AdaptiveTextInput()
                            {
                                Id="GetUserInputKeywords",
                                Placeholder="Please enter the keyword list separated by ',' Ex:RPA,FS ",
                                MaxLength=490,
                                IsMultiline=true
                            }
                        },
                    Actions = new List<AdaptiveAction>()
                        {
                            new AdaptiveSubmitAction()
                            {
                                Id = "contactSubmit",
                                Title = "Submit",
                                DataJson = "{ \"Type\": \"GetPPT\" }"
                            },
                            new AdaptiveOpenUrlAction()
                            {
                                Id="CallApi",
                                Url=new Uri("https://xyz"+"RPA")
                                //card.Actions.Card.AdaptiveTextInput.Placeholder
                            }
                        }
                }
            });

            card.Actions.Add(new AdaptiveShowCardAction()
            {
                Id = "GetPPTNo",
                Title = "No",
                Card = new AdaptiveCard()
                {
                    Body = new List<AdaptiveElement>()
                    {
                    },
                    Actions = new List<AdaptiveAction>()
                        {
                            new AdaptiveSubmitAction()
                            {
                                Id = "contactSubmit",
                                Title = "Submit",
                                DataJson = "{ \"Type\": \"GetPPTNo\" }"
                            }
                        }
                }
            });


            // Create the attachment with adapative card. 
            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }

        //https://blog.botframework.com/2018/07/12/how-to-properly-send-a-greeting-message-and-common-issues-from-customers/
        private async Task HandleSystemMessage(Activity message)
        {
            //await TelemetryLogger.TrackActivity(message);

            if (message.Type == ActivityTypes.Message)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {

            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing that the user is typing
            }
            //else if (message.Type == ActivityTypes.Ping)
            //{
            //}
            else if (message.Type == ActivityTypes.Event)
            {
                //var eventActivity = message.AsEventActivity();

                //if (eventActivity.Name == "setUserIdEvent")
                //{
                //    ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));

                //    var userId = message.From.Id;

                //    var welcomeMessages = GreetingHelper.FormatWelcomeMessages(userProfile);

                //    foreach (var welcomeMessage in welcomeMessages)
                //    {
                //        var reply = message.CreateReply(welcomeMessage);

                //        await connector.Conversations.ReplyToActivityAsync(reply);

                //    }

                //}
            }
        }

        private static async Task SendWelcomeMessageAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in turnContext.Activity.MembersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                   await turnContext.SendActivityAsync(
                        $"Welcome to Bot {member.Name}. {WelcomeText}",
                        cancellationToken: cancellationToken);

                    ////////////////////////////////
                    ///Add card on startup message

                    //var attachments = new List<Attachment>();
                    //var reply = MessageFactory.Attachment(attachments);
                    Attachment cardAttachment = null;
                    //if (text.Contains("testdc"))
                    //{
                        AdaptiveCard card = CreateHelloAdaptiveCard();
                        cardAttachment = new Attachment()
                        {
                            ContentType = AdaptiveCard.ContentType,
                            Content = card
                        };
                        //reply.Attachments.Add(cardAttachment);
                    //}
                    if (cardAttachment != null)
                    {
                        await turnContext.SendActivityAsync(MessageFactory.Attachment(cardAttachment), cancellationToken);
                    }
                }
            }
        }
        public static AdaptiveCard CreateHelloAdaptiveCard()
        {
            var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0));
            AdaptiveSubmitAction submitAction = new AdaptiveSubmitAction();
            submitAction.Title = "Hello";
            submitAction.Data = "Hello";

            card.Actions.Add(submitAction);            
            return card;
        }

        private static Attachment CreateAdaptiveCardAttachment(string filePath)
        {
            var adaptiveCardJson = File.ReadAllText(filePath);
            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCardJson),
            };
            return adaptiveCardAttachment;
        }
        public static AdaptiveCard CreateAdaptiveCard()
        {
            //Activity replyToConversation = message.CreateReply("Should go to conversation");
            //replyToConversation.Attachments = new List<Attachment>();

            AdaptiveCard card = new AdaptiveCard
            {
                // Specify speech for the card.
                Speak = "<s>Your  meeting about \"Adaptive Card design session\"<break strength='weak'/> is starting at 12:30pm</s><s>Do you want to snooze <break strength='weak'/> or do you want to send a late notification to the attendees?</s>"
            };

            // Add text to the card.
            card.Body.Add(new AdaptiveTextBlock()
            {
                Text = "Adaptive Card design session",
                Size = AdaptiveTextSize.Large,
                Weight = AdaptiveTextWeight.Bolder
            });

            // Add text to the card.
            card.Body.Add(new AdaptiveTextBlock()
            {
                Text = "Conf Room 112/3377 (10)"
            });

            // Add text to the card.
            card.Body.Add(new AdaptiveTextBlock()
            {
                Text = "12:30 PM - 1:30 PM"
            });

            // Add list of choices to the card.
            card.Body.Add(new AdaptiveChoiceSetInput()
            {
                Id = "snooze",
                Style = AdaptiveChoiceInputStyle.Compact,
                Choices = new List<AdaptiveChoice>()
    {
        new AdaptiveChoice() { Title = "5 minutes", Value = "5", IsSelected = true },
        new AdaptiveChoice() { Title = "15 minutes", Value = "15" },
        new AdaptiveChoice() { Title = "30 minutes", Value = "30" }
    }
            });

            // Add buttons to the card.
            card.Actions.Add(new AdaptiveOpenUrlAction()
            {
                Url = new Uri("http://foo.com"),
                Title = "Snooze"
            });

            card.Actions.Add(new AdaptiveOpenUrlAction()
            {
                Url = new Uri("http://foo.com"),
                Title = "I'll be late"
            });

            card.Actions.Add(new AdaptiveOpenUrlAction()
            {
                Url = new Uri("http://foo.com"),
                Title = "Dismiss"
            });

            // Create the attachment.
            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };

            return card;
            //replyToConversation.Attachments.Add(attachment);

            //var reply = await connector.Conversations.SendToConversationAsync(replyToConversation);

        }
        public static AdaptiveCard CreateAdapativecardWithColumn()
        {
            AdaptiveCard card = new AdaptiveCard
            {
                // Specify speech for the card.
                Speak = "<s>Your  meeting about \"Adaptive Card design session\"<break strength='weak'/> is starting at 12:30pm</s><s>Do you want to snooze <break strength='weak'/> or do you want to send a late notification to the attendees?</s>"
            };

            card.Body.Add(new AdaptiveColumnSet()
            {
                Id = "snooze",
                Columns = new List<AdaptiveColumn>()
                    {
                        new AdaptiveColumn()
                        {
                            //Size = AdaptiveColumnSize.Auto,
                            Items = new List<AdaptiveElement>()
                            {
                                new AdaptiveTextBlock()
                                {
                                    Text =  "Cat1",
                                    IsSubtle = true
                                },
                                new AdaptiveImage()
                                {
                                    Url = new Uri("https://adaptivecards.io/content/cats/1.png"),
                                    Size = AdaptiveImageSize.Auto,
                                    Style = AdaptiveImageStyle.Person
                                }
                            }
                        },
                        new AdaptiveColumn()
                        {
                            //Size = "300",
                            Items = new List<AdaptiveElement>()
                            {
                                new AdaptiveTextBlock()
                                {
                                    Text =  "Cat2",
                                    IsSubtle = true
                                },
                                new AdaptiveImage()
                                {
                                    Url = new Uri("https://adaptivecards.io/content/cats/1.png"),
                                    Size = AdaptiveImageSize.Auto,
                                    Style = AdaptiveImageStyle.Person
                                }

                            }
                        },
                        new AdaptiveColumn()
                        {
                            //Size = "300",
                            Items = new List<AdaptiveElement>()
                            {
                                new AdaptiveTextBlock()
                                {
                                    Text =  "Cat3",
                                    IsSubtle = true
                                },
                                new AdaptiveImage()
                                {
                                    Url = new Uri("https://adaptivecards.io/content/cats/1.png"),
                                    Size = AdaptiveImageSize.Auto,
                                    Style = AdaptiveImageStyle.Person
                                }

                            }
                        }
                    }
            });

            return card;
        }
        public static AdaptiveCard CreateAdaptiveCard_WithDynamicContent()
        {
            List<string> city = new List<string>() { "Delhi", "Bangalore", "Mumbai" };
            List<string> date = new List<string>() { "1-Jan", "26-Jan", "15-Aug" };
            List<string> des = new List<string>() { "New Year", "Republic Day", "Independence Day" };

            List<string> date1 = new List<string>() { "1-Jan", "26-Jan", "15-Aug", "25-Dec" };
            List<string> des1 = new List<string>() { "New Year", "Republic Day", "Independence Day", "Christmas Day" };

            List<string> date2 = new List<string>() { "1-Jan", "25-Dec" };
            List<string> des2 = new List<string>() { "New Year", "Christmas Day" };

            List<AdaptiveCard> cards = new List<AdaptiveCard>();
            cards.Add(HolidayListAdaptiveCard(date, des));
            cards.Add(HolidayListAdaptiveCard(date1, des1));
            cards.Add(HolidayListAdaptiveCard(date2, des2));

            var mainCard = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0));
            var column3 = new AdaptiveColumn();
            column3.Items.Add(new AdaptiveTextBlock() { Text = "Holiday City", Weight = AdaptiveTextWeight.Bolder });
            var columnSet1 = new AdaptiveColumnSet();
            columnSet1.Columns.Add(column3);
            var container1 = new AdaptiveContainer();
            container1.Style = AdaptiveContainerStyle.Emphasis;
            container1.Items.Add(columnSet1);
            mainCard.Body.Add(container1);

            List<AdaptiveShowCardAction> adaptiveShowCardActions = new List<AdaptiveShowCardAction>();
            for (int i = 0; i < city.Count; i++)
            {
                //mainCard.Actions.Add(new AdaptiveShowCardAction() { Title = city[i], Card = cards[i] });
                //mainCard.Actions.Add(new AdaptiveSubmitAction() {Title="Submit",Data= "Submit" });
                mainCard.Actions.Add(new AdaptiveSubmitAction() { Title = "Submit", Data = "Submit" });
                //{ \"Type\": \"GetPPTNo\" }
                //mainCard.Actions.Add(new AdaptiveOpenUrlAction() { Title = "OpenUrl", Url = new Uri("https://adaptivecards.io/explorer/Action.OpenUrl.html") });
            }

            var attachment = new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = mainCard
            };

            return mainCard;
            //var reply = MessageFactory.Attachment(attachment);
            //await stepContext.Context.SendActivityAsync(reply);
            //return new DialogTurnResult(DialogTurnStatus.Waiting);
        }
        private static AdaptiveCard HolidayListAdaptiveCard(List<string> date, List<string> description)
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

    }
}






