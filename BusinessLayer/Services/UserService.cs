using AutoMapper;
using DataAccessLayer.DTO;
using DataAccessLayer.Mappers;
using DataAccessLayer.Models;

namespace BusinessLayer.Services
{
    public class UserService : IUserService
    {
        IMemberMapper memberMapper;
        public UserService(IMemberMapper mapper)
        {
            memberMapper = mapper;
        }

        public async Task CreateUser(UserDTO userDTO)
        {
            try
            {
                // 속성 유효성검사
                // 업무규칙 적용
                // DTO와 Entity로 변경

                // Configure AutoMapper
                var configuration = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, USER>());

                // Perform mapping
                // Map<변경전의 Model, 변경후 Model>
                Mapper mapper = new Mapper(configuration);
                USER user = mapper.Map<UserDTO, USER>(userDTO);

                await memberMapper.Create(user);
                // Response..DTO 생성후 Controller로 전달
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Update
        public async Task UpdateUser(UserDTO userDTO)
        {
            try
            {
                // Configure AutoMapper
                var configuration = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, USER>());

                // Perform mapping
                // Map<변경전의 Model, 변경후 Model>
                Mapper mapper = new Mapper(configuration);
                USER user = mapper.Map<UserDTO, USER>(userDTO);

                // 데이터베이스에서 업데이트
                await memberMapper.Update(user);
            }
            catch (Exception ex)
            {
                throw new Exception("유저수정 에러발생", ex);
            }
        }

        // GetAll
        public async Task<List<UserDTO>> GetAllUsers()
        {
            try
            {
                var users = await memberMapper.GetAll(); 

                // AutoMapper
                var configuration = new MapperConfiguration(cfg => cfg.CreateMap<USER, UserDTO>());
                var mapper = new Mapper(configuration);

                // Entity -> DTO
                var userDTOList = mapper.Map< List<USER>, List<UserDTO>>(users);
                return userDTOList;
            }
            catch (Exception ex)
            {
                throw new Exception("유저목록조회 에러발생", ex);
            }
        }

        // GetById
        public async Task<UserDTO> GetUserById(int id)
        {
            try
            {
                var user = await memberMapper.GetById(id);
                // AutoMapper
                var configuration = new MapperConfiguration(cfg => cfg.CreateMap<USER, UserDTO>());
                var mapper = new Mapper(configuration);

                // Entity -> DTO
                var userDTO = mapper.Map<USER, UserDTO>(user);
                return userDTO;
            }
            catch (Exception ex)
            {
                throw new Exception($"유저조회 에러발생 ID: {id}", ex);
            }
        }

        // Delete
        public async Task<bool> DeleteUser(int id)
        {
            try
            {
                return await memberMapper.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"유저삭제 에러발생 ID: {id}.", ex);
            }
        }

    }
}
