using DataAccessLayer.Models;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace DataAccessLayer.Mappers
{
    public class FileMemberMapper : IMemberMapper
    {
        private readonly string _filePath;
        private int _currentId;
        private readonly object _fileLock = new object(); // 파일 동기화를 위한 객체

        public FileMemberMapper(IConfiguration configuration)
        {
            // "FileSettings:FilePath" 값을 IConfiguration에서 가져옴
            _filePath = configuration["FileSettings:FilePath"]
                        ?? throw new ArgumentNullException("FileSettings:FilePath is not configured");

            // 파일 경로의 디렉터리가 존재하지 않으면 생성
            var directory = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // 파일이 존재하지 않으면 초기화
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }

            // 파일에서 최대 ID를 가져와 초기화
            var users = LoadUsersFromFile();
            _currentId = users.Any() ? users.Max(u => u.Id) : 0;
        }

        public async Task<USER> Create(USER user)
        {
            var users = LoadUsersFromFile();

            user.Id = ++_currentId;
            users.Add(user);

            SaveUsersToFile(users);
            return await Task.FromResult(user);
        }

        public async Task<bool> Delete(int id)
        {
            var users = LoadUsersFromFile();

            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return await Task.FromResult(false);
            }

            users.Remove(user);
            SaveUsersToFile(users);
            return await Task.FromResult(true);
        }

        public async Task<List<USER>> GetAll()
        {
            var users = LoadUsersFromFile();
            return await Task.FromResult(users);
        }

        public async Task<USER> GetById(int? id)
        {
            if (id == null)
            {
                throw new Exception("ID는 NULL이 될 수 없습니다.");
            }

            var users = LoadUsersFromFile();

            var user = users.FirstOrDefault(u => u.Id == id.Value);
            if (user == null)
            {
                throw new Exception($"해당하는 유저가 존재하지 않습니다. ID {id}.");
            }

            return await Task.FromResult(user);
        }

        public async Task<USER> Update(USER user)
        {
            var users = LoadUsersFromFile();

            var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser == null)
            {
                throw new Exception($"해당하는 유저가 존재하지 않습니다. {user.Id}.");
            }

            // 기존 데이터를 수정
            existingUser.CpyName = user.CpyName;
            existingUser.Position = user.Position;
            existingUser.Username = user.Username;
            existingUser.Age = user.Age;

            SaveUsersToFile(users);
            return await Task.FromResult(existingUser);
        }

        // JSON 파일에서 사용자 데이터를 읽어오는 메서드
        private List<USER> LoadUsersFromFile()
        {
            lock (_fileLock)
            {
                // 파일이 없는 경우 빈 리스트 반환
                if (!File.Exists(_filePath))
                {
                    return new List<USER>();
                }

                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<USER>>(json) ?? new List<USER>();
            }
        }

        // 사용자 데이터를 JSON 파일에 저장하는 메서드
        private void SaveUsersToFile(List<USER> users)
        {
            lock (_fileLock)
            {
                var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
        }
    }
}