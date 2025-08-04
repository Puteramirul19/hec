using Hec.Entities;
using Hec.Settings;
using Hec.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Configuration;
using Hangfire;

namespace Hec.Notifications
{
    public class Notifier
    {
        private HecContext db;
        private SettingsStore settingsStore;
        private IEmailSender emailSender;
        private IBackgroundJobClient backgroundJob;

        public Notifier(HecContext db, SettingsStore settingsStore, IEmailSender emailSender, IBackgroundJobClient backgroundJob)
        {
            this.db = db;
            this.settingsStore = settingsStore;
            this.emailSender = emailSender;
            this.backgroundJob = backgroundJob;
        }

        //-------------------------------------------------------------------------------- 
        //  Notifications and Reminders
        //-------------------------------------------------------------------------------- 

        private Friendship TryFindFriend(Guid id)
        {
            var friend = db.Friendships
                            .Include(x => x.Invitee)
                            .Include(x => x.Inviter)
                            .FirstOrDefault(x => x.Id == id);
            if (friend == null)
                throw new IdNotFoundException<Friendship>(id);

            return friend;
        }

        // Send accepted friend request to inviter
        public void FriendInviteAccepted(Guid friendId)
        {
            var friend = TryFindFriend(friendId);
            SendToUser(friend, friend.Inviter.Email, EmailTemplate.FriendInviteAccepted_Inviter);
        }

        // Send friend request to invitee
        public void FriendInviteSubmitted(Guid friendId)
        {
            var friend = TryFindFriend(friendId);
            SendToUser(friend, friend.Invitee.Email, EmailTemplate.FriendInviteSubmitted_Invitee);
        }

        private void Remind(Friendship friend, bool checking, Action action, Expression<Action<Notifier>> job, DateTimeOffset interval)
        {
            if (checking)
            {
                action();
                if (interval != DateTimeOffset.MinValue)
                {
                    backgroundJob.Schedule<Notifier>(job, interval);
                }
            }
        }

        private EmailTemplate TryFindEmailTemplate(string id)
        {
            var template = db.EmailTemplates.Find(id);
            if (template == null)
                throw new IdNotFoundException<EmailTemplate>(id);
            return template;
        }

        private void SendEmailTemplate(string toAddress, string toName, string templateId, Friendship friend, IEnumerable<string> attachmentFileIds, DateTime scheduledOn)
        {
            var parsed = ParsedTemplate.Parse(TryFindEmailTemplate(templateId), friend);
            AsyncHelper.RunSync(() => emailSender.SendEmail(toAddress, toName, parsed.Subject, parsed.Content, attachmentFileIds, scheduledOn));
        }

        private void SendEmailTemplate(IEnumerable<User> users, string templateId, Friendship friend, IEnumerable<string> attachmentFileIds, DateTime scheduledOn)
        {
            var parsed = ParsedTemplate.Parse(TryFindEmailTemplate(templateId), friend);
            foreach (var user in users)
            {
                AsyncHelper.RunSync(() => emailSender.SendEmail(user.Email, user.FullName, parsed.Subject, parsed.Content, attachmentFileIds, scheduledOn));
            }
        }

        private void SendToUser(Friendship friend, string userId, string templateId)
        {
            var user = db.Users.FirstOrDefault(x => x.Email == userId);
            if (user == null)
                throw new IdNotFoundException<User>(userId);

            SendToUser(friend, user.Email, user.FullName, templateId);
        }

        private void SendToUser(Friendship friend, string toAddress, string toName, string templateId)
        {
            SendEmailTemplate(toAddress,
                                toName,
                                templateId,
                                friend,
                                null, DateTime.Now);
        }
    }
}
