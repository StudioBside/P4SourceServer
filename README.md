# P4SourceServer

[![Build Status](https://github.com/StudioBside/P4SourceServer/workflows/Build%20Solution/badge.svg)](https://github.com/StudioBside/P4SourceServer/actions)
[![GitHub Package Registry](https://github.com/StudioBside/P4SourceServer/workflows/Publish%20NuGet%20Package/badge.svg)](https://github.com/StudioBside/P4SourceServer/packages)

## GetP4Revisions 도구

이 프로젝트는 Perforce 리비전을 가져오는 도구를 제공합니다.

## NuGet 도구로 설치하기

이 도구는 .NET 글로벌 도구로 설치할 수 있습니다:

```bash
dotnet tool install --global GetP4Revisions --version {version}
```

## GitHub 패키지 레지스트리에서 설치하기

GitHub 패키지 레지스트리에서 설치하려면, 다음과 같이 nuget.config 파일을 구성해야 합니다:

1. `nuget.config.example` 파일을 `nuget.config`로 복사합니다.
2. `OWNER`를 리포지토리 소유자 이름으로 변경합니다.
3. `USERNAME`과 `TOKEN`을 GitHub 사용자 이름과 개인 액세스 토큰으로 변경합니다.

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="github" value="https://nuget.pkg.github.com/OWNER/index.json" />
  </packageSources>
  <packageSourceCredentials>
    <github>
      <add key="Username" value="USERNAME" />
      <add key="ClearTextPassword" value="TOKEN" />
    </github>
  </packageSourceCredentials>
</configuration>
```

그런 다음 다음 명령으로 도구를 설치할 수 있습니다:

```bash
dotnet tool install --global GetP4Revisions --version {version}
```

## 개발자를 위한 정보

### GitHub Actions 설정

이 프로젝트는 GitHub Actions를 사용하여 자동으로 빌드 및 패키지 배포를 수행합니다. 이를 위해 다음과 같은 시크릿을 GitHub 저장소에 설정해야 합니다:

1. `STUDIOBSIDE_PAT`: StudioBside GitHub 조직의 패키지에 액세스할 수 있는 Personal Access Token
   - GitHub 프로필 → Settings → Developer settings → Personal access tokens → Generate new token
   - 권한: `read:packages` 
   - 토큰을 생성한 후 GitHub 저장소의 Settings → Secrets and variables → Actions에서 추가

### 릴리스 생성 방법

1. 변경 사항을 커밋합니다.
2. 태그를 생성합니다 (예: `git tag v0.0.1`).
3. 태그를 푸시합니다 (예: `git push origin v0.0.1`).

GitHub Actions 워크플로우가 자동으로 NuGet 패키지를 빌드하고 게시합니다.
