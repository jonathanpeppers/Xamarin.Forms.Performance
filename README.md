# Xamarin.Forms.Performance
Repo for testing performance in Xamarin.Forms on Android

Current Times from `Application.OnCreate` to `MainActivity.OnResume`:

| Device  | Configuration | Linker Setting | Time             |
|---------|---------------|----------------|------------------|
| Pixel 2 | Release       | Link All       | 00:00:00.4532682 |
|         |               |                | 00:00:00.4487269 |
|         |               |                | 00:00:00.4492285 |
| x86 emu | Release       | Link All       | 00:00:00.4595582 |
|         |               |                | 00:00:00.3954876 |
|         |               |                | 00:00:00.4845893 |

***XamlC is on***
