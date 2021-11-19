﻿namespace GameFoundation.Scripts.Network.ApiHandler
{
    using GameFoundation.Scripts.GameManager;
    using GameFoundation.Scripts.Models;
    using GameFoundation.Scripts.Network.Authentication;
    using GameFoundation.Scripts.Network.WebService;
    using GameFoundation.Scripts.Utilities.LogService;
    using MechSharingCode.WebService.Authentication;

    public class LoginHttpRequest : BaseHttpRequest<LoginHttpResponseData>
    {
        private readonly DataLoginServices       dataLoginServices;
        private readonly GameFoundationLocalData localData;
        private readonly HandleLocalDataServices handleLocalDataServices;
        private readonly PlayerState             mechPlayerState;

        public LoginHttpRequest(ILogService logger, DataLoginServices dataLoginServices, GameFoundationLocalData localData,
            HandleLocalDataServices handleLocalDataServices, PlayerState mechPlayerState) : base(logger)
        {
            this.dataLoginServices       = dataLoginServices;
            this.localData               = localData;
            this.handleLocalDataServices = handleLocalDataServices;
            this.mechPlayerState         = mechPlayerState;
        }
        public override void Process(LoginHttpResponseData responseData)
        {
            var jwtToken       = responseData.JwtToken;
            var refreshToken   = responseData.RefreshToken;
            var expirationTime = responseData.ExpirationTime;
            if (string.IsNullOrEmpty(jwtToken)) return;
            this.SaveDataToLocalData(jwtToken, refreshToken, expirationTime);
        }

        private void SaveDataToLocalData(string jwtToken, string refreshToken, long expirationTime)
        {
            this.localData.ServerToken.JwtToken       = jwtToken;
            this.localData.ServerToken.RefreshToken   = refreshToken;
            this.localData.ServerToken.ExpirationTime = expirationTime;

            switch (this.dataLoginServices.currentTypeLogin)
            {
                case TypeLogIn.Facebook:
                    this.localData.UserDataLogin.LastLogin = TypeLogIn.Facebook;
                    this.mechPlayerState.PlayerData.Name   = this.localData.UserDataLogin.FacebookLogin.UserName;
                    this.mechPlayerState.PlayerData.Avatar = this.localData.UserDataLogin.FacebookLogin.URLImage;
                    break;
                case TypeLogIn.Google:
                    this.localData.UserDataLogin.LastLogin = TypeLogIn.Google;
                    this.mechPlayerState.PlayerData.Name   = this.localData.UserDataLogin.GoogleLogin.UserName;
                    this.mechPlayerState.PlayerData.Avatar = this.localData.UserDataLogin.GoogleLogin.URLImage;
                    break;
                case TypeLogIn.None:
                    break;
            }

            this.handleLocalDataServices.SaveLocalDataToString(this.localData);
            this.dataLoginServices.Status.Value = AuthenticationStatus.Authenticated;
        }

        public override void ErrorProcess(int statusCode)
        {
            switch (statusCode)
            {
                case 1:
                    // ToDo
                    break;
                case 2:
                    this.dataLoginServices.Status.Value = this.dataLoginServices.currentTypeLogin == TypeLogIn.Google ? AuthenticationStatus.FailWithGoogleToken : AuthenticationStatus.FailWithFbToken;
                    break;
                case 3:
                    this.dataLoginServices.Status.Value = AuthenticationStatus.FailWithRefreshToken;
                    break;
                default:
                    base.ErrorProcess(statusCode);
                    break;
            }
        }
    }
}