using DataAccessLayer.Models;
using System.Collections.Concurrent;

namespace DataAccessLayer.Mappers
{
    public class InMemoryMemberMapper : IMemberMapper
    {
        // readonly: 참조를 바꿀 수 없다.
        private readonly ConcurrentDictionary<int, USER> _users = new ConcurrentDictionary<int, USER>();
        private int _currentId = 0;

        public Task<USER> Create(USER user)
        {
            // Thread-safe 방식으로 값을 1 증가시키고, 증가된 값을 반환 
            // ref 키워드로 _currentId를 참조로 전달하여 원본 값을 직접 수정
            user.Id = Interlocked.Increment(ref _currentId);
            // 메모리 기반 딕셔너리로 user의 ID를 키값으로 저장
            _users[user.Id] = user;
            // 계산된 결과를 비동기 작업으로 반환 (실제로는 동기 방식으로 데이터 반환?)
            return Task.FromResult(user);
        }

        public Task<bool> Delete(int id)
        {
            var removed = _users.TryRemove(id, out _);
            return Task.FromResult(removed);
        }

        public Task<List<USER>> GetAll()
        {
            var userList = _users.Values.ToList();
            return Task.FromResult(userList);
        }

        public Task<USER> GetById(int? id)
        {
            if (id == null || !_users.TryGetValue(id.Value, out var user))
            {
                throw new Exception($"No user found with ID {id}.");
            }

            return Task.FromResult(user);
        }

        public Task<USER> Update(USER user)
        {
            if (!_users.ContainsKey(user.Id))
            {
                throw new Exception($"No user found with ID {user.Id}.");
            }

            _users[user.Id] = user;
            return Task.FromResult(user);
        }
    }
}
