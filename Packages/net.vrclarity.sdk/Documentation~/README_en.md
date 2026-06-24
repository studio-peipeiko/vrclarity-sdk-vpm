# VRClarity SDK Setup Guide

An SDK for VRChat worlds that measures player behavior and sends it to VRClarity.

> 📖 **For full setup steps, metric details, and troubleshooting, always refer to the online documentation.**
> This file keeps only a brief overview; the details live online to avoid duplicated maintenance.

## Documentation

- **SDK Setup Guide (full, illustrated steps)**: https://vrclarity.net/docs/sdk-guide
- **SDK Overview (metric types and how to read them)**: https://vrclarity.net/docs/sdk
- **Privacy Policy**: https://vrclarity.net/docs/privacy
- **Service Terms of Service**: https://vrclarity.net/docs/terms
- **SDK Terms of Service (data ownership, prohibitions, etc.)**: [ToS_en.md](../../../ToS_en.md)

## Quick Start

1. Add the registry to VCC:
   ```
   https://studio-peipeiko.github.io/vrclarity-sdk-vpm/index.json
   ```
2. Install **VRClarity SDK** from **Manage Project**
3. Issue a **Key ID** and **Encryption Key** on the [VRClarity Dashboard](https://vrclarity.net) under "SDK API Key Management"
4. Right-click the Hierarchy → **VRClarity > Create Tracker** (places the Tracker and a Notice Panel)
5. Enter the Key ID / Encryption Key in the Inspector
6. **Check "Enable Tracking" in the Inspector just before uploading**, then build and upload your world as usual (no data is collected while it is unchecked)

> It can take up to about 1 hour for data to appear on the dashboard.

See the [SDK Setup Guide](https://vrclarity.net/docs/sdk-guide) for detailed, illustrated steps and troubleshooting.

## Requirements

- Unity 2022.3 or later
- VRChat SDK Worlds 3.7.0 or later (UdonSharp bundled)

## License

MIT License ([LICENSE](../../../LICENSE))
