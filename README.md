# App


JWT configuration: set `Token:Key` with .NET User Secrets in Development, or `Token__Key` in the deployment environment (at least 32 UTF-8 bytes). Token creation and validation use the same key. Never commit the signing key.
