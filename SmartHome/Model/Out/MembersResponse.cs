using Domain;

namespace Model.Out;

public class MembersResponse
{
    public List<MemberResponse> Members { get; set; }

    public MembersResponse(List<Member> members)
    {
        Members = members.Select(member => new MemberResponse(member)).ToList();
    }

    public override bool Equals(object? obj)
    {
        return obj is MembersResponse response &&
               Members.SequenceEqual(response.Members);
    }
}