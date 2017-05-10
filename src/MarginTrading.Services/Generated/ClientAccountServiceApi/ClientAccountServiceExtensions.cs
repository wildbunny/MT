// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace MarginTrading.Services.Generated.ClientAccountServiceApi
{
    using System.Threading.Tasks;
   using Models;

    /// <summary>
    /// Extension methods for ClientAccountService.
    /// </summary>
    public static partial class ClientAccountServiceExtensions
    {
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            public static IClientAccount ApiClientAccountsRegisterPost(this IClientAccountService operations, RegisterRequest request = default(RegisterRequest))
            {
                return System.Threading.Tasks.Task.Factory.StartNew(s => ((IClientAccountService)s).ApiClientAccountsRegisterPostAsync(request), operations, System.Threading.CancellationToken.None, System.Threading.Tasks.TaskCreationOptions.None, System.Threading.Tasks.TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async System.Threading.Tasks.Task<IClientAccount> ApiClientAccountsRegisterPostAsync(this IClientAccountService operations, RegisterRequest request = default(RegisterRequest), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
            {
                using (var _result = await operations.ApiClientAccountsRegisterPostWithHttpMessagesAsync(request, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            public static bool? ApiClientAccountsIsTraderWithEmailExistsPost(this IClientAccountService operations, IsTraderWithEmailExistsRequest request = default(IsTraderWithEmailExistsRequest))
            {
                return System.Threading.Tasks.Task.Factory.StartNew(s => ((IClientAccountService)s).ApiClientAccountsIsTraderWithEmailExistsPostAsync(request), operations, System.Threading.CancellationToken.None, System.Threading.Tasks.TaskCreationOptions.None, System.Threading.Tasks.TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async System.Threading.Tasks.Task<bool?> ApiClientAccountsIsTraderWithEmailExistsPostAsync(this IClientAccountService operations, IsTraderWithEmailExistsRequest request = default(IsTraderWithEmailExistsRequest), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
            {
                using (var _result = await operations.ApiClientAccountsIsTraderWithEmailExistsPostWithHttpMessagesAsync(request, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            public static IClientAccount ApiClientAccountsAuthenticatePost(this IClientAccountService operations, AuthenticateRequest request = default(AuthenticateRequest))
            {
                return System.Threading.Tasks.Task.Factory.StartNew(s => ((IClientAccountService)s).ApiClientAccountsAuthenticatePostAsync(request), operations, System.Threading.CancellationToken.None, System.Threading.Tasks.TaskCreationOptions.None, System.Threading.Tasks.TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async System.Threading.Tasks.Task<IClientAccount> ApiClientAccountsAuthenticatePostAsync(this IClientAccountService operations, AuthenticateRequest request = default(AuthenticateRequest), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
            {
                using (var _result = await operations.ApiClientAccountsAuthenticatePostWithHttpMessagesAsync(request, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            public static void ApiClientAccountsChangePasswordPost(this IClientAccountService operations, ChangePasswordRequest request = default(ChangePasswordRequest))
            {
                System.Threading.Tasks.Task.Factory.StartNew(s => ((IClientAccountService)s).ApiClientAccountsChangePasswordPostAsync(request), operations, System.Threading.CancellationToken.None, System.Threading.Tasks.TaskCreationOptions.None,  System.Threading.Tasks.TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async System.Threading.Tasks.Task ApiClientAccountsChangePasswordPostAsync(this IClientAccountService operations, ChangePasswordRequest request = default(ChangePasswordRequest), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
            {
                await operations.ApiClientAccountsChangePasswordPostWithHttpMessagesAsync(request, null, cancellationToken).ConfigureAwait(false);
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            public static void ApiClientAccountsChangePhonePost(this IClientAccountService operations, ChangePhoneRequest request = default(ChangePhoneRequest))
            {
                System.Threading.Tasks.Task.Factory.StartNew(s => ((IClientAccountService)s).ApiClientAccountsChangePhonePostAsync(request), operations, System.Threading.CancellationToken.None, System.Threading.Tasks.TaskCreationOptions.None,  System.Threading.Tasks.TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async System.Threading.Tasks.Task ApiClientAccountsChangePhonePostAsync(this IClientAccountService operations, ChangePhoneRequest request = default(ChangePhoneRequest), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
            {
                await operations.ApiClientAccountsChangePhonePostWithHttpMessagesAsync(request, null, cancellationToken).ConfigureAwait(false);
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            public static IClientAccount ApiClientAccountsGetByIdPost(this IClientAccountService operations, GetByIdRequest request = default(GetByIdRequest))
            {
                return System.Threading.Tasks.Task.Factory.StartNew(s => ((IClientAccountService)s).ApiClientAccountsGetByIdPostAsync(request), operations, System.Threading.CancellationToken.None, System.Threading.Tasks.TaskCreationOptions.None, System.Threading.Tasks.TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async System.Threading.Tasks.Task<IClientAccount> ApiClientAccountsGetByIdPostAsync(this IClientAccountService operations, GetByIdRequest request = default(GetByIdRequest), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
            {
                using (var _result = await operations.ApiClientAccountsGetByIdPostWithHttpMessagesAsync(request, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            public static System.Collections.Generic.IList<IClientAccount> ApiClientAccountsGetByIdsPost(this IClientAccountService operations, GetByIdsRequest request = default(GetByIdsRequest))
            {
                return System.Threading.Tasks.Task.Factory.StartNew(s => ((IClientAccountService)s).ApiClientAccountsGetByIdsPostAsync(request), operations, System.Threading.CancellationToken.None, System.Threading.Tasks.TaskCreationOptions.None, System.Threading.Tasks.TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async System.Threading.Tasks.Task<System.Collections.Generic.IList<IClientAccount>> ApiClientAccountsGetByIdsPostAsync(this IClientAccountService operations, GetByIdsRequest request = default(GetByIdsRequest), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
            {
                using (var _result = await operations.ApiClientAccountsGetByIdsPostWithHttpMessagesAsync(request, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            public static IClientAccount ApiClientAccountsGetByEmailAndPartnerIdPost(this IClientAccountService operations, GetByEmailAndPartnerIdRequest request = default(GetByEmailAndPartnerIdRequest))
            {
                return System.Threading.Tasks.Task.Factory.StartNew(s => ((IClientAccountService)s).ApiClientAccountsGetByEmailAndPartnerIdPostAsync(request), operations, System.Threading.CancellationToken.None, System.Threading.Tasks.TaskCreationOptions.None, System.Threading.Tasks.TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async System.Threading.Tasks.Task<IClientAccount> ApiClientAccountsGetByEmailAndPartnerIdPostAsync(this IClientAccountService operations, GetByEmailAndPartnerIdRequest request = default(GetByEmailAndPartnerIdRequest), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
            {
                using (var _result = await operations.ApiClientAccountsGetByEmailAndPartnerIdPostWithHttpMessagesAsync(request, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            public static System.Collections.Generic.IList<IClientAccount> ApiClientAccountsGetByEmailPost(this IClientAccountService operations, GetByEmailRequest request = default(GetByEmailRequest))
            {
                return System.Threading.Tasks.Task.Factory.StartNew(s => ((IClientAccountService)s).ApiClientAccountsGetByEmailPostAsync(request), operations, System.Threading.CancellationToken.None, System.Threading.Tasks.TaskCreationOptions.None, System.Threading.Tasks.TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async System.Threading.Tasks.Task<System.Collections.Generic.IList<IClientAccount>> ApiClientAccountsGetByEmailPostAsync(this IClientAccountService operations, GetByEmailRequest request = default(GetByEmailRequest), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
            {
                using (var _result = await operations.ApiClientAccountsGetByEmailPostWithHttpMessagesAsync(request, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            public static string ApiClientAccountsGenerateNotificationsIdPost(this IClientAccountService operations, GenerateNotificationsIdRequest request = default(GenerateNotificationsIdRequest))
            {
                return System.Threading.Tasks.Task.Factory.StartNew(s => ((IClientAccountService)s).ApiClientAccountsGenerateNotificationsIdPostAsync(request), operations, System.Threading.CancellationToken.None, System.Threading.Tasks.TaskCreationOptions.None, System.Threading.Tasks.TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async System.Threading.Tasks.Task<string> ApiClientAccountsGenerateNotificationsIdPostAsync(this IClientAccountService operations, GenerateNotificationsIdRequest request = default(GenerateNotificationsIdRequest), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
            {
                using (var _result = await operations.ApiClientAccountsGenerateNotificationsIdPostWithHttpMessagesAsync(request, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            public static bool? ApiClientAccountsIsPasswordCorrectPost(this IClientAccountService operations, IsPasswordCorrectRequest request = default(IsPasswordCorrectRequest))
            {
                return System.Threading.Tasks.Task.Factory.StartNew(s => ((IClientAccountService)s).ApiClientAccountsIsPasswordCorrectPostAsync(request), operations, System.Threading.CancellationToken.None, System.Threading.Tasks.TaskCreationOptions.None, System.Threading.Tasks.TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async System.Threading.Tasks.Task<bool?> ApiClientAccountsIsPasswordCorrectPostAsync(this IClientAccountService operations, IsPasswordCorrectRequest request = default(IsPasswordCorrectRequest), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
            {
                using (var _result = await operations.ApiClientAccountsIsPasswordCorrectPostWithHttpMessagesAsync(request, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            public static void ApiClientAccountsSetPinPost(this IClientAccountService operations, SetPinRequest request = default(SetPinRequest))
            {
                System.Threading.Tasks.Task.Factory.StartNew(s => ((IClientAccountService)s).ApiClientAccountsSetPinPostAsync(request), operations, System.Threading.CancellationToken.None, System.Threading.Tasks.TaskCreationOptions.None,  System.Threading.Tasks.TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async System.Threading.Tasks.Task ApiClientAccountsSetPinPostAsync(this IClientAccountService operations, SetPinRequest request = default(SetPinRequest), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
            {
                await operations.ApiClientAccountsSetPinPostWithHttpMessagesAsync(request, null, cancellationToken).ConfigureAwait(false);
            }

    }
}