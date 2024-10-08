using Domain.Concrete;

namespace Model.Out;

public class GetMembersResponse
{
    public List<GetMemberResponse> Members { get; set; }

    public GetMembersResponse(List<Member> members)
    {
        Members = members.Select(member => new GetMemberResponse(member)).ToList();
    }

    public override bool Equals(object? obj)
    {
        return obj is GetMembersResponse response &&
               Members.SequenceEqual(response.Members);
    }
}