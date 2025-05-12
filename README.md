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

#### Personal Access Token 생성 방법:

1. **GitHub에 로그인** - StudioBside 조직에 소속된 계정으로 로그인합니다.
2. **프로필 메뉴** - 우측 상단의 프로필 아이콘을 클릭하고 `Settings`를 선택합니다.
3. **개발자 설정** - 왼쪽 메뉴 하단에서 `Developer settings`를 클릭합니다.
4. **Personal Access Token** - `Personal access tokens` → `Tokens (classic)`을 선택합니다.
5. **새 토큰 생성** - `Generate new token (classic)` 버튼을 클릭합니다.
6. **토큰 설정**:
   - `Note`: "P4SourceServer CI/CD" 등 용도를 알 수 있는 이름을 입력합니다.
   - `Expiration`: 적절한 유효기간을 설정합니다(자동화 용도라면 장기간 권장).
   - `Scopes`: 다음 권한이 필요합니다:
     - `read:packages`: 패키지 읽기 권한
     - `write:packages`: 패키지 쓰기 권한
     - `delete:packages`: 필요시 패키지 삭제 권한
     - `workflow`: 워크플로우 실행 권한
     - `repo`: (private 저장소의 경우) 저장소 접근 권한
7. **토큰 생성** - `Generate token` 버튼을 클릭합니다.
8. **토큰 저장** - 생성된 토큰을 즉시 안전한 곳에 복사해 저장합니다.

#### GitHub Secrets에 토큰 등록 방법:

1. P4SourceServer 저장소의 `Settings` 탭으로 이동합니다.
2. 왼쪽 사이드바에서 `Secrets and variables` → `Actions`를 클릭합니다.
3. `New repository secret` 버튼을 클릭합니다.
4. 이름은 `STUDIOBSIDE_PAT`로 입력하고, 값에는 앞서 생성한 토큰을 붙여넣습니다.
5. `Add secret` 버튼을 클릭합니다.

### 패키지 발행 문제 해결

패키지 발행 시 다음과 같은 오류가 발생할 경우:

```
warn : Your request could not be authenticated by the GitHub Packages service.
error: Response status code does not indicate success: 403 (Forbidden).
```

1. **PAT 권한 확인**: 반드시 `write:packages` 권한이 포함되어 있어야 합니다.
2. **저장소 설정 확인**: GitHub 저장소 `Settings` -> `Packages` 메뉴에서 패키지 설정이 올바른지 확인합니다.
3. **조직 정책 확인**: StudioBside 조직에서 패키지 게시 권한이 있는지 확인합니다.

만약 StudioBside 조직에 게시할 권한이 없다면, 본인 소유의 저장소에 게시한 후 StudioBside 조직의 프로젝트에서 참조할 수 있습니다. 이 경우 워크플로우 파일의 `github.repository_owner` 부분을 자신의 GitHub 계정으로 수정하세요.

### 릴리스 생성 방법

1. 변경 사항을 커밋합니다.
2. 태그를 생성합니다 (예: `git tag v0.0.1`).
3. 태그를 푸시합니다 (예: `git push origin v0.0.1`).

GitHub Actions 워크플로우가 자동으로 NuGet 패키지를 빌드하고 게시합니다.
