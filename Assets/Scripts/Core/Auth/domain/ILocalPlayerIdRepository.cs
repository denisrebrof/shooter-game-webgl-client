using Core.Auth.domain.model;

namespace Core.Auth.domain
{
    public interface ILocalPlayerIdRepository
    {
        bool Fetch(out string id);
        void Set(string id);
    }
}