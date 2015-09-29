using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net.Mime;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using System.Globalization;

namespace Mantadt.RallyCrawler
{
    public class SendEmail
    {
        /// <summary>
        /// Get or Set SmtpServer
        /// </summary>
        public string SmtpServer { get; set; }
        /// <summary>
        /// Get or Set Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Get or Set FromAddresser
        /// </summary>
        public string FromAddresser { get; set; }
        /// <summary>
        /// Get or Set FromAddresserDelegate
        /// </summary>
        public string FromAddresserDelegate { get; set; }
        /// <summary>
        /// Get or Set FromAddresserDelegateDisplay
        /// </summary>
        public string FromAddresserDelegateDisplay { get; set; }
        /// <summary>
        /// Get or Set FromAddresserDisplay
        /// </summary>
        public string FromAddresserDisplay { get; set; }
        /// <summary>
        /// Get or Set ToAddressees
        /// </summary>
        public List<KeyValuePair<string, string>> ToAddressees { get; set; }
        /// <summary>
        /// Get or Set CCAddressees
        /// </summary>
        public List<KeyValuePair<string, string>> CCAddressees { get; set; }
        /// <summary>
        /// Get or Set BccAddressees
        /// </summary>
        public List<KeyValuePair<string, string>> BccAddressees { get; set; }
        /// <summary>
        /// Get or Set OptionalAddressees
        /// </summary>
        public List<KeyValuePair<string, string>> OptionalAddressees { get; set; }
        /// <summary>
        /// Get or Set MsgSubject
        /// </summary>
        public string MsgSubject { get; set; }
        /// <summary>
        /// Get or Set MsgBody
        /// </summary>
        public StringBuilder MsgBody { get; set; }
        /// <summary>
        /// Get or Set MeetingStartTime
        /// </summary>
        public DateTime MeetingStartTime { get; set; }
        /// <summary>
        /// Get or Set MeetingEndTime
        /// </summary>
        public DateTime MeetingEndTime { get; set; }
        /// <summary>
        /// Get or Set Location
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// Get or Set ETimeZone
        /// </summary>
        public int ETimeZone { get; set; }
        public enum EInviteType
        {
            None = 0,
            JustMail = 1,
            OneOffInvite = 2,
            RecurringInvite = 3
        }
        /// <summary>
        /// Get or Set InviteType
        /// </summary>
        public EInviteType InviteType { get; set; }
        /// <summary>
        /// Get or Set IsCancellation
        /// </summary>
        public bool IsCancellation { get; set; }
        
        /// <summary>
        /// SMTPClient object initiation
        /// </summary>
        SmtpClient sc;

        /// <summary>
        /// Constractor for SendEmail Class
        /// </summary>
        public SendEmail()
        {
            this.SmtpServer = ConfigurationManager.AppSettings["CEM_SMTPSERVER"].ToString();

            SMPTPserverDetails();

            SMPTPCredinals();

            SMTPEnableSSl();
        }

        
        /// <summary>
        /// Provide details to send mail invite
        /// </summary>
        /// <exception cref="C2EM.Exceptions.SendEscalationMailException">To send an escalation mail</exception>
        public void SendMailInvite()
        {
            try
            {
                MailMessage msg = new MailMessage();

                msg.From = new MailAddress(this.FromAddresser.Trim(), this.FromAddresserDisplay.Trim());

                foreach (KeyValuePair<string, string> kvpTo in this.ToAddressees)
                {
                    msg.To.Add(AddMailAddress(kvpTo.Key.ToString().Trim(),
                        kvpTo.Value.ToString().Trim()));

                }

                if (this.InviteType == EInviteType.JustMail)
                {
                    if (this.CCAddressees != null)
                    {
                        foreach (KeyValuePair<string, string> kvpTo in this.CCAddressees)
                        {
                            msg.CC.Add(AddMailAddress(kvpTo.Key.ToString().Trim(),
                                kvpTo.Value.ToString().Trim()));
                        }
                    }
                    if (this.BccAddressees != null)
                    {
                        foreach (KeyValuePair<string, string> kvpTo in this.BccAddressees)
                        {
                            msg.Bcc.Add(AddMailAddress(kvpTo.Key.ToString().Trim(),
                                kvpTo.Value.ToString().Trim()));
                        }
                    }
                }

                msg.Subject = this.MsgSubject;

                /*System.Net.Mime.ContentType HTMLType = new System.Net.Mime.ContentType("text/html; charset=UTF-8");
                AlternateView HTMLView = AlternateView.CreateAlternateViewFromString(this.MsgBody.ToString(),
                    HTMLType);
                HTMLView.LinkedResources.Add(SmtpServerConfigInfo.CEMBackground);
                HTMLView.LinkedResources.Add(SmtpServerConfigInfo.CEMHeader);
                HTMLView.LinkedResources.Add(SmtpServerConfigInfo.CEMHeaderRight);
                HTMLView.LinkedResources.Add(SmtpServerConfigInfo.CEMHeaderDown);
                HTMLView.LinkedResources.Add(SmtpServerConfigInfo.CEMFooter);
                
                msg.AlternateViews.Add(HTMLView); */

                if (this.InviteType != EInviteType.JustMail)
                {
                    StringBuilder str = new StringBuilder();
                    str.AppendLine("BEGIN:VCALENDAR");
                    str.AppendLine("PRODID:-//Cognizant CEM");
                    str.AppendLine("VERSION:1.0");
                    if (this.IsCancellation)
                    {
                        str.AppendLine("METHOD:CANCEL");
                        str.AppendLine("STATUS:CANCELLED");
                    }
                    else
                    {
                        str.AppendLine("METHOD:REQUEST");
                    }
                    str.AppendLine("BEGIN:VEVENT");

                    if (this.MeetingStartTime != null)
                    {
                        str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}",
                            this.MeetingStartTime.AddMinutes((double)this.ETimeZone * -1)));
                    }
                    str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
                    if (this.MeetingEndTime != null)
                    {
                        str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}",
                            this.MeetingEndTime.AddMinutes((double)this.ETimeZone * -1)));
                    }

                    if (this.InviteType == EInviteType.RecurringInvite)
                    {
                        str.AppendLine(string.Format("RRULE:W1 MO TU WE TH FR {0:HHmm}",
                            this.MeetingStartTime.AddMinutes((double)this.ETimeZone * -1)));
                    }

                    str.AppendLine("LOCATION:" + this.Location.Trim());
                    str.AppendLine("CATEGORIES:MEETING");
                    str.AppendLine(string.Format("UID:{0}", this.Id));
                    str.AppendLine(string.Format("DESCRIPTION:{0}", msg.Body));
                    str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", msg.Body));
                    str.AppendLine("X-MICROSOFT-CDO-BUSYSTATUS:BUSY");
                    str.AppendLine("X-MICROSOFT-CDO-IMPORTANCE:Normal");
                    str.AppendLine("X-MICROSOFT-DISALLOW-COUNTER:TRUE");
                    str.AppendLine("X-MS-OLK-ALLOWEXTERNCHECK:TRUE");
                    str.AppendLine(string.Format("X-MS-OLK-APPTSEQTIME:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
                    str.AppendLine("X-MS-OLK-AUTOFILLLOCATION:TRUE");
                    str.AppendLine("X-MS-OLK-CONFTYPE:0");
                    str.AppendLine(string.Format("SUMMARY:{0}", msg.Subject));

                    str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", msg.From.Address));

                    for (int iCount = 0; iCount < msg.To.Count; iCount++)
                    {
                        str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";" +
                        "ROLE=ATTENDEE;EXPECT=REQUIRE;RSVP=TRUE:mailto:{1}",
                        msg.To[iCount].DisplayName, msg.To[iCount].Address));
                    }

                    if (this.OptionalAddressees != null)
                    {
                        foreach (KeyValuePair<string, string> kvpTo in this.OptionalAddressees)
                        {
                            str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";" +
                                "ROLE=OPT-PARTICIPANT;EXPECT=REQUEST;RSVP=TRUE:mailto:{1}"
                                , kvpTo.Value.Trim(), kvpTo.Key.Trim()));
                        }
                    }

                    str.AppendLine("BEGIN:VALARM");
                    str.AppendLine("TRIGGER:-PT15M");
                    str.AppendLine("ACTION:DISPLAY");
                    str.AppendLine("DESCRIPTION:Reminder");
                    str.AppendLine("END:VALARM");
                    str.AppendLine("END:VEVENT");
                    str.AppendLine("END:VCALENDAR");
                    System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType("text/calendar");
                    if (this.IsCancellation)
                    {
                        ct.Parameters.Add("method", "CANCEL");
                    }
                    else
                    {
                        ct.Parameters.Add("method", "REQUEST");
                    }
                    ct.Parameters.Add("name", "meeting.ics");
                    AlternateView avCal = AlternateView.CreateAlternateViewFromString(str.ToString(), ct);
                    msg.AlternateViews.Add(avCal);

                    str = null;
                    ct = null;
                    avCal = null;
                }

                sc.Send(msg);                
                sc = null;
                msg = null;
            }            
            catch (Exception ex)
            {
                throw ex;
            }           
        }

        /// <summary>
        /// Set the smptp server credinals 
        /// </summary>
        private void SMPTPCredinals()
        {
            try
            {
                if (string.IsNullOrEmpty(SmtpServerConfigInfo.GetSmtpUserId.Trim())
                    || string.IsNullOrEmpty(SmtpServerConfigInfo.GetSmtpPwd.Trim()))
                    sc.UseDefaultCredentials = true;
                //else if (SmtpServerConfigInfo.GetSmtpDomain.Trim().Equals(string.Empty))
                else if (string.IsNullOrEmpty(SmtpServerConfigInfo.GetSmtpDomain.Trim()))
                {
                    var crediential = new System.Net.NetworkCredential();
                    crediential.UserName = SmtpServerConfigInfo.GetSmtpUserId.Trim();
                    crediential.Password = SmtpServerConfigInfo.GetSmtpPwd.Trim();
                    sc.Credentials = crediential;

                }
                else
                {
                    var crediential = new System.Net.NetworkCredential();
                    crediential.UserName = SmtpServerConfigInfo.GetSmtpUserId.Trim();
                    crediential.Password = SmtpServerConfigInfo.GetSmtpPwd.Trim();
                    crediential.Domain = SmtpServerConfigInfo.GetSmtpDomain.Trim();
                    sc.Credentials = crediential;
                }
            }
            //catch (Exception)
            catch (InvalidOperationException)
            {
                sc.UseDefaultCredentials = true;
            }
            catch (SmtpException)
            {
                sc.UseDefaultCredentials = true;
            }
        }

        /// <summary>
        /// To Set the SMTP server details
        /// </summary>
        /// <exception cref="C2EM.Exceptions.SendEscalationMailException">To send an escalation mail</exception>
        private void SMPTPserverDetails(){
            try
            {
                //if (!SmtpServerConfigInfo.GetSmtpPort.Trim().Equals(string.Empty))
                if (!string.IsNullOrEmpty(SmtpServerConfigInfo.GetSmtpPort.Trim()))
                    sc = new SmtpClient(this.SmtpServer.Trim(),
                        Convert.ToInt32(SmtpServerConfigInfo.GetSmtpPort.Trim()));
                else
                    sc = new SmtpClient(this.SmtpServer.Trim());
            }
            //catch (Exception)
            catch (SmtpException)
            {
                sc = new SmtpClient(this.SmtpServer.Trim());
            }
            catch (NullReferenceException nullexp)
            {
                throw nullexp;
            }
        }

        /// <summary>
        /// To Set the SMTP SSLEnabled 
        /// </summary>
        private void SMTPEnableSSl()
        {
            try
            {
                if (!string.IsNullOrEmpty(SmtpServerConfigInfo.IsSSLEnabled.Trim())
                    && Convert.ToInt32(SmtpServerConfigInfo.IsSSLEnabled.Trim()) == 1)
                    sc.EnableSsl = true;
                else
                    sc.EnableSsl = false;
            }
            //catch (Exception)
            catch (SmtpException smtpex)
            {
                throw smtpex;
            }
            catch (NullReferenceException nullexp)
            {
                throw nullexp;
            }
        }

        /// <summary>
        /// Get the Mail Address
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private MailAddress AddMailAddress(string key, string value)
        {
            return new MailAddress(key, value);
        }
    }
}
