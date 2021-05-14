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

    public class AdaptiveCardsBot1 : ActivityHandler
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
            //Random r = new Random();
            //var cardAttachment = CreateAdaptiveCardAttachment(_cards[r.Next(_cards.Length)]);

            ////turnContext.Activity.Attachments = new List<Attachment>() { cardAttachment };
            //await turnContext.SendActivityAsync(MessageFactory.Attachment(cardAttachment), cancellationToken);
            //await turnContext.SendActivityAsync(MessageFactory.Text("Please enter any text to see another card."), cancellationToken);

            //1.
            //turnContext.Activity.RemoveRecipientMention();
            //var text = turnContext.Activity.Text.Trim().ToLower();

            //Attachment cardAttachment = null;
            //if (text.Contains("flight"))
            //    cardAttachment = CreateAdaptiveCardAttachment(_cards[0]);
            //else if (text.Contains("image"))
            //    cardAttachment = CreateAdaptiveCardAttachment(_cards[1]);
            //else if (text.Contains("weather"))
            //    cardAttachment = CreateAdaptiveCardAttachment(_cards[2]);
            //else if (text.Contains("restaurant"))
            //    cardAttachment = CreateAdaptiveCardAttachment(_cards[3]);
            //else
            //    cardAttachment = CreateAdaptiveCardAttachment(_cards[4]);

            ////turnContext.Activity.Attachments = new List<Attachment>() { cardAttachment };
            //await turnContext.SendActivityAsync(MessageFactory.Attachment(cardAttachment), cancellationToken);
            //await turnContext.SendActivityAsync(MessageFactory.Text("Please enter any text to see another card."), cancellationToken);

            // //2.
            // turnContext.Activity.RemoveRecipientMention();
            // var text = turnContext.Activity.Text.Trim().ToLower();

            // // Cards are sent as Attachments in the Bot Framework.
            // // So we need to create a list of attachments for the reply activity.
            // var attachments = new List<Attachment>();

            // // Reply to the activity we received with an activity.
            // var reply = MessageFactory.Attachment(attachments);

            // // Decide which type of card(s) we are going to show the user
            // Attachment cardAttachment = null;
            // if (text.Contains("adaptive"))
            // {
            //     // Display an Adaptive Card
            //     AdaptiveCard card = CreateAdaptiveCard();
            //     // Create the attachment.
            //     cardAttachment = new Attachment()
            //     {
            //         ContentType = AdaptiveCard.ContentType,
            //         Content = card
            //     };
            //     reply.Attachments.Add(cardAttachment);
            //     //reply.Attachments.Add(GetAudioCard().ToAttachment());
            // }
            //else if (text.Contains("testcolumn"))
            // {
            //     // Display an Adaptive Card
            //     AdaptiveCard card = CreateAdapativecardWithColumn();
            //     // Create the attachment.
            //     cardAttachment = new Attachment()
            //     {
            //         ContentType = AdaptiveCard.ContentType,
            //         Content = card
            //     };
            //     reply.Attachments.Add(cardAttachment);
            //     //reply.Attachments.Add(GetAudioCard().ToAttachment());
            // }

            //3.
            turnContext.Activity.RemoveRecipientMention();
            var text = turnContext.Activity.Text.Trim().ToLower();
            var attachments = new List<Attachment>();
            var reply = MessageFactory.Attachment(attachments);
            Attachment cardAttachment = null;
            //if (text.Contains("testdc"))
            if (text.Contains("hello"))
            {
                AdaptiveCard card = CreateAdaptiveCard_WithDynamicContent();
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
                await turnContext.SendActivityAsync(MessageFactory.Text("Please enter any text to see another card."), cancellationToken);
            }

           
        if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                var token = JToken.Parse(turnContext.Activity.ChannelData.ToString());
                string selectedcolor = string.Empty;
                if (System.Convert.ToBoolean(token["postback"].Value<string>()))
                {
                    JToken commandToken = JToken.Parse(turnContext.Activity.Value.ToString());
                    string command = commandToken["action"].Value<string>();

                    if (command.ToLowerInvariant() == "colorselector")
                    {
                        selectedcolor = commandToken["choiceset"].Value<string>();
                    }

                }

                await turnContext.SendActivityAsync($"You Selected {selectedcolor}", cancellationToken: cancellationToken);

            }
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

        //private async Task<DialogTurnResult> ShowCard(WaterfallStepContext stepContext, CancellationToken cancellationToken)
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
            var column1 = new AdaptiveColumn();
            var column2 = new AdaptiveColumn();

            AdaptiveCardHelper.AddItemToAdaptiveColumn(column, AdaptiveCardHelper.AddTextBlock("Sr. No"));
            AdaptiveCardHelper.AddItemToAdaptiveColumn(column, AdaptiveCardHelper.AddTextBlock("Date"));
            AdaptiveCardHelper.AddItemToAdaptiveColumn(column, AdaptiveCardHelper.AddTextBlock("Description"));

            for (int i = 0; i < date.Count; i++)
            {
                AdaptiveCardHelper.AddItemToAdaptiveColumn(column, AdaptiveCardHelper.AddTextBlock((i + 1).ToString()));
                AdaptiveCardHelper.AddItemToAdaptiveColumn(column, AdaptiveCardHelper.AddTextBlock((date[i]).ToString()));
                AdaptiveCardHelper.AddItemToAdaptiveColumn(column, AdaptiveCardHelper.AddTextBlock((description[i]).ToString()));
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

        //private AdaptiveCard HolidayListAdaptiveCard(List<string> date, List<string> description)
        //{
        //    var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0));
        //    List<AdaptiveColumn> columns = new List<AdaptiveColumn>();
        //    var column = new AdaptiveColumn();
        //    var column1 = new AdaptiveColumn();
        //    var column2 = new AdaptiveColumn();


        //    var textBlock = new AdaptiveTextBlock();
        //    textBlock.Text = "Sr. No";
        //    textBlock.Weight = AdaptiveTextWeight.Bolder;
        //    textBlock.Size = AdaptiveTextSize.Large;
        //    textBlock.Color = AdaptiveTextColor.Accent;
        //    column.Items.Add(textBlock);

        //    var textBlock1 = new AdaptiveTextBlock();
        //    textBlock1.Text = "Date";
        //    textBlock1.Weight = AdaptiveTextWeight.Bolder;
        //    textBlock1.Size = AdaptiveTextSize.Large;
        //    textBlock1.Color = AdaptiveTextColor.Good;
        //    column1.Items.Add(textBlock1);

        //    var textBlock2 = new AdaptiveTextBlock();
        //    textBlock2.Text = "Description";
        //    textBlock2.Weight = AdaptiveTextWeight.Bolder;
        //    textBlock2.Size = AdaptiveTextSize.Large;
        //    textBlock2.Color = AdaptiveTextColor.Dark;
        //    column2.Items.Add(textBlock2);

        //    for (int i = 0; i < date.Count; i++)
        //    {
        //        column.Items.Add(new AdaptiveTextBlock() { Text = (i + 1).ToString() });
        //        column1.Items.Add(new AdaptiveTextBlock() { Text = date[i] });
        //        column2.Items.Add(new AdaptiveTextBlock() { Text = description[i] });
        //    }

        //    var columnSet = new AdaptiveColumnSet();
        //    columnSet.Columns.Add(column);
        //    columnSet.Columns.Add(column1);
        //    columnSet.Columns.Add(column2);
        //    var container = new AdaptiveContainer();
        //    container.Style = AdaptiveContainerStyle.Emphasis;
        //    container.Items.Add(columnSet);
        //    card.Body.Add(container);
        //    return card;
        //}

        //Others R&D
        //protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        //{
        //    turnContext.Activity.RemoveRecipientMention();
        //    var text = turnContext.Activity.Text.Trim().ToLower();

        //    if (text.Contains("mention"))
        //        await MentionActivityAsync(turnContext, cancellationToken);
        //    else if (text.Contains("who"))
        //                await GetSingleMemberAsync(turnContext, cancellationToken);
        //            else if (text.Contains("update"))
        //                await CardActivityAsync(turnContext, true, cancellationToken);
        //            else if (text.Contains("message"))
        //                await MessageAllMembersAsync(turnContext, cancellationToken);
        //            else if (text.Contains("delete"))
        //                await DeleteCardActivityAsync(turnContext, cancellationToken);
        //            else
        //                await CardActivityAsync(turnContext, false, cancellationToken);
        //}
        private async Task MentionActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var mention = new Mention
            {
                Mentioned = turnContext.Activity.From,
                Text = $"<at>{XmlConvert.EncodeName(turnContext.Activity.From.Name)}</at>",
            };

            var replyActivity = MessageFactory.Text($"Hello {mention.Text}.");
            replyActivity.Entities = new List<Entity> { mention };

            await turnContext.SendActivityAsync(replyActivity, cancellationToken);
        }
        private async Task GetSingleMemberAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var member = new TeamsChannelAccount();

            try
            {
                member = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);
            }
            catch (ErrorResponseException e)
            {
                if (e.Body.Error.Code.Equals("MemberNotFoundInConversation"))
                {
                    await turnContext.SendActivityAsync("Member not found.");
                    return;
                }
                else
                {
                    throw e;
                }
            }

            var message = MessageFactory.Text($"You are: {member.Name}.");
            var res = await turnContext.SendActivityAsync(message);

        }
        private async Task CardActivityAsync(ITurnContext<IMessageActivity> turnContext, bool update, CancellationToken cancellationToken)
        {

            var card = new HeroCard
            {
                Buttons = new List<CardAction>
                        {
                            new CardAction
                            {
                                Type = ActionTypes.MessageBack,
                                Title = "Message all members",
                                Text = "MessageAllMembers"
                            },
                            new CardAction
                            {
                                Type = ActionTypes.MessageBack,
                                Title = "Who am I?",
                                Text = "whoami"
                            },
                            new CardAction
                            {
                                Type = ActionTypes.MessageBack,
                                Title = "Delete card",
                                Text = "Delete"
                            }
                        }
            };


            if (update)
            {
                await SendUpdatedCard(turnContext, card, cancellationToken);
            }
            else
            {
                await SendWelcomeCard(turnContext, card, cancellationToken);
            }

        }
        private static async Task SendUpdatedCard(ITurnContext<IMessageActivity> turnContext, HeroCard card, CancellationToken cancellationToken)
        {
            card.Title = "I've been updated";

            var data = turnContext.Activity.Value as JObject;
            data = JObject.FromObject(data);
            data["count"] = data["count"].Value<int>() + 1;
            card.Text = $"Update count - {data["count"].Value<int>()}";

            card.Buttons.Add(new CardAction
            {
                Type = ActionTypes.MessageBack,
                Title = "Update Card",
                Text = "UpdateCardAction",
                Value = data
            });

            var activity = MessageFactory.Attachment(card.ToAttachment());
            activity.Id = turnContext.Activity.ReplyToId;

            await turnContext.UpdateActivityAsync(activity, cancellationToken);
        }
        private static async Task SendWelcomeCard(ITurnContext<IMessageActivity> turnContext, HeroCard card, CancellationToken cancellationToken)
        {
            var initialValue = new JObject { { "count", 0 } };
            card.Title = "Welcome!";
            card.Buttons.Add(new CardAction
            {
                Type = ActionTypes.MessageBack,
                Title = "Update Card",
                Text = "UpdateCardAction",
                Value = initialValue
            });

            var activity = MessageFactory.Attachment(card.ToAttachment());

            await turnContext.SendActivityAsync(activity, cancellationToken);
        }
        private async Task MessageAllMembersAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var teamsChannelId = turnContext.Activity.TeamsGetChannelId();
            var serviceUrl = turnContext.Activity.ServiceUrl;
            var credentials = new MicrosoftAppCredentials(_appId, _appPassword);
            ConversationReference conversationReference = null;

            var members = await GetPagedMembers(turnContext, cancellationToken);

            foreach (var teamMember in members)
            {
                var proactiveMessage = MessageFactory.Text($"Hello {teamMember.GivenName} {teamMember.Surname}. I'm a Teams conversation bot.");

                var conversationParameters = new ConversationParameters
                {
                    IsGroup = false,
                    Bot = turnContext.Activity.Recipient,
                    Members = new ChannelAccount[] { teamMember },
                    TenantId = turnContext.Activity.Conversation.TenantId,
                };

                await ((BotFrameworkAdapter)turnContext.Adapter).CreateConversationAsync(
                    teamsChannelId,
                    serviceUrl,
                    credentials,
                    conversationParameters,
                    async (t1, c1) =>
                    {
                        conversationReference = t1.Activity.GetConversationReference();
                        await ((BotFrameworkAdapter)turnContext.Adapter).ContinueConversationAsync(
                            _appId,
                            conversationReference,
                            async (t2, c2) =>
                            {
                                await t2.SendActivityAsync(proactiveMessage, c2);
                            },
                            cancellationToken);
                    },
                    cancellationToken);
            }

            await turnContext.SendActivityAsync(MessageFactory.Text("All messages have been sent."), cancellationToken);
        }
        private static async Task<List<TeamsChannelAccount>> GetPagedMembers(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var members = new List<TeamsChannelAccount>();
            string continuationToken = null;

            do
            {
                var currentPage = await TeamsInfo.GetPagedMembersAsync(turnContext, 100, continuationToken, cancellationToken);
                continuationToken = currentPage.ContinuationToken;
                members = members.Concat(currentPage.Members).ToList();
            }
            while (continuationToken != null);

            return members;
        }
        private async Task DeleteCardActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            await turnContext.DeleteActivityAsync(turnContext.Activity.ReplyToId, cancellationToken);
        }
    }
}






