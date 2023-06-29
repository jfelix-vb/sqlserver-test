# SQL Server test connector

Checks SQL server service connection to a remote server. The application attempts to open a connection and execute a single call, namely:

`
SELECT @VERSION()
`

The outcome must be the SQL Server version banner, an output sample is show below.

## Usage

```
sqlserver-test --help
```

### Help output:
```
Description:
  Tests a connection with a legacy SQL Server and prints the remote server version banner.

Usage:
  sqlserver-test [options]

Options:
  --host <host>          SQL Server host name or IP.
  --port <port>          The TCP port for connection (default 1433).
  --database <database>  The initial database to use.
  --uid <uid>            User login.
  --password <password>  User password.
  --version              Show version information
  -?, -h, --help         Show help and usage information
```

## Command line example:

```
sqlserver-test --host "172.28.192.1" --port "1435" --database "master" --uid "sysgls" --password "<password>"
```

Note: default TCP port for SQL Server is `1433`.

### Outcome:

```
Microsoft SQL Server 2012 (SP2-GDR) (KB3194719) - 11.0.5388.0 (X64)
Sep 23 2016 16:56:29
Copyright (c) Microsoft Corporation
Standard Edition (64-bit) on Windows NT 6.3 <X64> (Build 9600: ) (Hypervisor)
```

## Build

To build with shared libraries usage:

```
dotnet publish --configuration Release -r linux-x64 --no-self-contained
```