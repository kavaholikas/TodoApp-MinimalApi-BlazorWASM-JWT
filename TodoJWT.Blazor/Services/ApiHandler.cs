using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using TodoJWT.Models;

namespace TodoJWT.Blazor.Services
{
    public class ApiHandler
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;

        public ApiHandler(HttpClient http, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
        {
            _http = http;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<ApiHandlerResponse> RegisterUser(LoginRegisterDto registerModel)
        {
            return await _sendUserDetails("/user/register", registerModel);
        }

        public async Task<ApiHandlerResponse> LoginUser(LoginRegisterDto loginModel)
        {
            return await _sendUserDetails("/user/login", loginModel);
        }

        public async void LogoutUser()
        {
            await _localStorage.RemoveItemAsync("token");
            await _authStateProvider.GetAuthenticationStateAsync();
        }

        public async Task<ApiHandlerResponse> GetUserTasks()
        {
            try
            {
                List<TodoTaskDto>? tasks = await _http.GetFromJsonAsync<List<TodoTaskDto>>("/tasks");
                return new ApiHandlerResponse(true) { Tasks = tasks };
            }
            catch (HttpRequestException ex)
            {
                string message = ex.Message;    
                return new ApiHandlerResponse(false, message.Replace("\"", ""));
            }
        }

        public async Task<ApiHandlerResponse> CreateUserTask(TodoTaskDto task)
        {
            HttpResponseMessage response = await _http.PostAsJsonAsync<TodoTaskDto>("/tasks/create", task);

            if (response.IsSuccessStatusCode)
            {
                return new ApiHandlerResponse(true);
            }
            else
            {
                return new ApiHandlerResponse(false, "Something went wrong.");
            }
        }

        public async Task<ApiHandlerResponse> CompleteUserTask(int id)
        {
            HttpResponseMessage response = await _http.PatchAsync($"/tasks/complete/{id}", null);

            if (response.IsSuccessStatusCode)
            {
                return new ApiHandlerResponse(true);
            }
            else
            {
                return new ApiHandlerResponse(false, "Something went wrong");
            }
        }

        public async Task<ApiHandlerResponse> RemoveUserTask(int id)
        {
            HttpResponseMessage response = await _http.DeleteAsync($"/tasks/delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                return new ApiHandlerResponse(true);
            }
            else
            {
                return new ApiHandlerResponse(false, "Something went wrong");
            }
        }

        public async Task<ApiHandlerResponse> ArchiveUserTask(int id)
        {
            HttpResponseMessage response = await _http.PatchAsync($"/tasks/archive/{id}", null);

            if (response.IsSuccessStatusCode)
            {
                return new ApiHandlerResponse(true);
            }
            else
            {
                return new ApiHandlerResponse(false, "Something went wrong");
            }
        }

        public async Task<ApiHandlerResponse> GetUserArchivedTasks()
        {
            try
            {
                List<TodoTaskDto>? tasks = await _http.GetFromJsonAsync<List<TodoTaskDto>>("/tasks/archive");
                return new ApiHandlerResponse(true) { Tasks = tasks };

            }
            catch (Exception)
            {
                return new ApiHandlerResponse(false, "Something went wrong");
            }
        }

        public async Task<ApiHandlerResponse> GetUsers()
        {
            try
            {
                List<UserDto>? users = await _http.GetFromJsonAsync<List<UserDto>>("/user/users");

                return new ApiHandlerResponse(true)
                {
                    Users = users
                };
            }
            catch (Exception)
            {
                return new ApiHandlerResponse(false, "Something went wrong");
            }
        }

        public async Task<ApiHandlerResponse> DeleteUser(int id)
        {
            try
            {
                HttpResponseMessage response = await _http.DeleteAsync($"/user/delete/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return new ApiHandlerResponse(true);
                }
                else
                {
                    return new ApiHandlerResponse(false, "Something went wrong");
                }

            }
            catch (Exception)
            {
                return new ApiHandlerResponse(false, "Something went wrong");
            }
        }

        public async Task<ApiHandlerResponse> ChangeUserPassword(ChangePasswordDto changepasswordModel)
        {
            HttpResponseMessage response = await _http.PutAsJsonAsync<ChangePasswordDto>("/user/change-password", changepasswordModel);

            if (response.IsSuccessStatusCode)
            {
                string token = await response.Content.ReadAsStringAsync();
                await _localStorage.SetItemAsStringAsync("token", token);
                await _authStateProvider.GetAuthenticationStateAsync();

                return new ApiHandlerResponse(true);
            }
            else
            {
                string message = await response.Content.ReadAsStringAsync();
                return new ApiHandlerResponse(false, message.Replace("\"", ""));
            }

        }

        private async Task<ApiHandlerResponse> _sendUserDetails(string endpoint, LoginRegisterDto userModel)
        {
            HttpResponseMessage response = await _http.PostAsJsonAsync<LoginRegisterDto>(endpoint, userModel);

            if (response.IsSuccessStatusCode)
            {
                await _extractToken(response);
                return new ApiHandlerResponse(true);
            }
            else
            {
                string message = await response.Content.ReadAsStringAsync();
                return new ApiHandlerResponse(false, message.Replace("\"", ""));
            }
        }

        private async Task _extractToken(HttpResponseMessage response)
        {
            string token = await response.Content.ReadAsStringAsync();
            await _localStorage.SetItemAsStringAsync("token", token);
            await _authStateProvider.GetAuthenticationStateAsync();
        }
    }

    public class ApiHandlerResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public List<TodoTaskDto>? Tasks { get; set; }
        public List<UserDto>? Users { get; set; }

        public ApiHandlerResponse(bool success, string message = "")
        {
            Success = success;
            Message = message;
        }
    }
}
