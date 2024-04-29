using Microsoft.AspNetCore.SignalR;

namespace SignalR.Server.Hubs;

public record MessagePayload(Guid UserId, string Content);

public record Message
{
    public Guid MessageId { get; init; }
    public Guid UserId { get; init; }
    public string Content { get; init; }
    public DateTime SentAt { get; init; }
    public bool IsDelivered { get; private set; }

    public Message(Guid messageId, Guid userId, string content, DateTime sentAt)
    {
        MessageId = messageId;
        UserId = userId;
        Content = content;
        SentAt = sentAt;
        IsDelivered = false;
    }

    public void SetDelivered()
        => IsDelivered = true;
}

public class MessageHub : Hub
{
    private const string UserIdString = "E3EC1513-864C-41E9-8B97-ED746666C9B3";
    private static readonly Guid UserId = Guid.Parse(UserIdString);

    private static readonly List<Message> Messages =
    [
        new Message(Guid.NewGuid(), UserId, "Hello", DateTime.Now),
        new Message(Guid.NewGuid(), UserId, "Yet another hello", DateTime.Now)
    ];

    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();
        await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());

        await Clients
            .Group(userId.ToString())
            .SendAsync("ReceiveAllMessages", Messages);
    }

    public async Task SendMessage(MessagePayload payload)
    {
        var message = new Message(Guid.NewGuid(), payload.UserId, payload.Content, DateTime.Now);

        var userId = GetUserId();
        await Clients
            .Group(userId.ToString())
            .SendAsync("ReceiveMessage", message);
    }

    public Task MessagesDelivered(List<Guid> messageIds)
    {
        messageIds.ForEach(messageId => Messages.FirstOrDefault(m => m.MessageId == messageId)?.SetDelivered());
        return Task.CompletedTask;
    }

    private Guid GetUserId()
    {
        var httpContext = Context.GetHttpContext();
        var userId = httpContext!.Request.Query["UserId"];
        return Guid.Parse(userId.ToString());
    }
}