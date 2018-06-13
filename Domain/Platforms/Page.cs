using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PB.BL.Domain.Platform
{
    [Table("tblPage")]
    public class Page : ICloneable
    {
        [Key] public int PageId { get; set; }

        [Required] public string PageName { get; set; }

        [Required] public string Title { get; set; }

        public virtual List<Tag> Tags { get; set; }

        public int SubplatformId { get; set; }

        [Required] public Subplatform Subplatform { get; set; }

        public static List<Page> GetDefaultPages(Subplatform subplatform)
        {
            return new List<Page>
            {
                new Page
                {
                    PageName = "Home",
                    Title = "Home",
                    Tags = new List<Tag>
                    {
                        new Tag
                        {
                            Name = "BannerTitle",
                            Text = subplatform.Name
                        },
                        new Tag
                        {
                            Name = "BannerTextSub1",
                            Text = "Volg uw favoriete mensen, organisaties en thema's en bekijk hoe deze door anderen besproken worden op sociale media."
                        },
                        new Tag
                        {
                            Name = "BannerTextSub2",
                            Text = "Creeër uw eigen dashboard en bekijk en analyseer live grafieken!"
                        },
                        new Tag
                        {
                            Name = "call-to-action-text",
                            Text = "Krijg toegang tot ons duizelingwekkend aanbod aan geanalyseerde en gevisualiseerde data."
                        }
                    },
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "Dashboard",
                    Title = "Dashboard",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "WeeklyReview",
                    Title = "Weekly Review",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                 new Page
                {
                    PageName = "Items",
                    Title = "Items",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "Persons",
                    Title = "Personen",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "Organisations",
                    Title = "Organisaties",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "Themes",
                    Title = "Thema's",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "Account",
                    Title = "Account",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "FAQ",
                    Title = "FAQ",
                    Subplatform = subplatform,
                    Tags = new List<Tag>
                    {
                        new Tag
                        {
                            Name = "Question1",
                            Text = "Waarom zou ik me registreren op de Politieke Barometer?",
                            Text2 = "De politieke barometer kan u bijstaan om politieke trends te monitoren en te analyseren. Hiermee kan u uw visie op politieke events en trends aanscherpen, waardoor u ook bewuster wordt van wat er rondom je gebeurt. Dit zal er voor zorgen dat u met zekerheid naar de stemhokjes kan."
                        },
                        new Tag
                        {
                            Name="Question2",
                            Text = "Hoe maak ik een account aan?",
                            Text2 = "Klik Rechtsboven op het login / register. Wij verwerken uw gegevens en u kan direct aan de slag!"
                        },
                        new Tag
                        {   Name ="Question3",
                            Text= "Hoe subscribe ik op een item?",
                            Text2 = "Rechts boven vindt u een zoekbalk waarin in de naam van het gewenste thema, persoon of organisatie kan ingevult worden. Dit zal u leiden tot aan de desbetreffende detailpagina waar een subscribe knop in het menu staat."
                        },
                        new Tag
                        {
                            Name = "Question4",
                            Text = "Kan ik mijn wachtwoord of gebruikersnaam nog aanpassen?",
                            Text2=  "Ja, dit gaat! Als u naar je persoonlijke instellingen gaat dan kan je al deze gegevens up-to-date houden."
                        },
                        new Tag
                        {
                            Name = "Question5",
                            Text = "Kan ik mijn account ook verwijderen?",
                            Text2 = "Ja, dit gaat ook in je persoonlijke instellingen. We vinden het echter wel spijtig om je te zien vertrekken. Het zou ons veel plezier doen moest je het contact formulier invullen en je ervaringen met het platform delen."
                        },
                        new Tag
                        {
                            Name = "Question6",
                            Text = "Hoe werkt politieke barometer?",
                            Text2 =  "Wij werken nauw samen met TextGain, een bedrijf dat Web services aanbied voor voorspellende text analyses. Zij analyseren Twitter-berichten voor ons en wij bieden u met veel plezier deze data aan in een overzichtelijk formaat."
                        },
                        new Tag
                        {
                            Name = "Question7",
                            Text = "Ik vind jullie kleurenschema maar niets. Kan ik dit aanpassen?",
                            Text2 ="Ja, dit gaat. U kan rechtsboven uw thema aanpassen naar een van onze 3 thema's. Indien u geen enkel van onze thema's leuk vindt, kan u altijd een verzoek sturen via onze contact pagina."
                        }
                    }
                },
                new Page
                {
                    PageName = "Contact",
                    Title = "Contact",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "ItemDetail",
                    Title = "Item Detail",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "Login",
                    Title = "Login",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "Register",
                    Title = "Register",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "Notification",
                    Title = "Notifications",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "UserSettings",
                    Title = "User Settings",
                    Tags = new List<Tag>(),
                    Subplatform = subplatform
                },
                new Page
                {
                    PageName = "Menu",
                    Title = "Menu",
                    Tags = new List<Tag>
                    {
                        new Tag
                        {
                            Name = "Home",
                            Text = "Home"
                        },
                        new Tag
                        {
                            Name = "Dashboard",
                            Text = "Dashboard"
                        },
                        new Tag
                        {
                            Name = "Weekly_Review",
                            Text = "Weekly Review"
                        },
                        new Tag
                        {
                            Name = "Account",
                            Text = "My Account"
                        },
                        new Tag
                        {
                            Name = "More",
                            Text = "More"
                        },
                        new Tag
                        {
                            Name = "FAQ",
                            Text = "FAQ"
                        },
                        new Tag
                        {
                            Name = "Contact",
                            Text = "Contact"
                        },
                        new Tag
                        {
                            Name = "Items",
                            Text = "Items"
                        },
                        new Tag
                        {
                            Name = "Persons",
                            Text = "Personen"
                        },
                        new Tag
                        {
                            Name = "Organisations",
                            Text = "Organisaties"
                        },
                        new Tag
                        {
                            Name = "Themes",
                            Text = "Thema's"
                        },
                        new Tag
                        {
                            Name = "Legal",
                            Text = "Terms of use"
                        }
                    },
                    Subplatform = subplatform
                }
            };
        }

        public object Clone()
        {
            return new Page()
            {
                PageId = PageId,
                PageName = PageName,
                Title = Title,
                Tags = Tags.Select(t => (Tag)t.Clone()).ToList(),
                SubplatformId = SubplatformId,
                Subplatform = Subplatform
            };
        }

        public override bool Equals(object obj)
        {
            return obj is Page page &&
                   PageName == page.PageName &&
                   EqualityComparer<Subplatform>.Default.Equals(Subplatform, page.Subplatform);
        }

        public override int GetHashCode()
        {
            var hashCode = -562850145;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PageName);
            hashCode = hashCode * -1521134295 + EqualityComparer<Subplatform>.Default.GetHashCode(Subplatform);
            return hashCode;
        }
    }
}