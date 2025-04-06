using EventPlanner.Application.Interfaces;
using EventPlanner.Domain.Entity;

namespace EventPlanner.Application.Services.EmailService;
public class EmailService(IRepository<Email> emailRepository) : IEmailService
{
    private IRepository<Email> _emailRepository= emailRepository;
    public async Task<bool> sendInvitaionEmail(string To, Event eventDetails)
    {
        var message = $"You have been invited to {eventDetails.Name} event.";
        var status = await _emailRepository.Add(
            new Email 
            {
                To = To,
                Content = message,
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
            }
        );
        return status;
    }
}
