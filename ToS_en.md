# VRClarity SDK Terms of Service

**Last Updated**: June 22, 2026
**Version**: 1.3.0

These Terms of Service (the "Terms") govern your use of the VRClarity SDK (the "SDK") provided by studio peipeiko ("we", "us", or "our"). By using the SDK, you agree to be bound by these Terms.

## 1. Service Overview

The VRClarity SDK is a player behavior measurement and analytics service for VRChat worlds. World creators can use the SDK to automatically measure, transmit, and analyze the following data:

- Stay duration and travel distance milestones
- Visit count tracking with bucket system
- Platform detection (PCVR / Desktop / Quest / Android / iOS)
- Time-series player count tracking

### 1.1 Notice to Visiting Players (Important)

When deploying the SDK in a world, world creators are **strongly encouraged** to inform visiting players that "VRClarity is used to collect non-personally-identifying statistical data (such as stay duration and travel distance)." When you create a Tracker, the SDK automatically generates and places a "Notice Panel" that informs visiting players of this data collection by default. World creators should ensure this notice by keeping the Notice Panel in a location visible to visiting players. For details on data handling, please refer to the [Privacy Policy](https://vrclarity.net/docs/privacy) and the [Service Terms of Service](https://vrclarity.net/docs/terms).

## 2. Data Ownership

### 2.1 Ownership of Transmitted Data
**Rights in the aggregated and statistical datasets — and works derived therefrom — created from metrics data transmitted to VRClarity belong to VRClarity (us).** World creators consent to the transmission and aggregated use of data by using the SDK, but do not claim rights in the aggregated or statistical datasets. The rights that individual data subjects (such as visiting players) hold under applicable data protection laws are in no way limited by this (see the following paragraph).

For clarity, the "ownership" referred to in this section concerns rights in the aggregated and statistical datasets and works derived therefrom, and does not limit or negate the rights that individual data subjects (such as visiting players) may have under the Privacy Policy and applicable data protection laws (including the EU General Data Protection Regulation (GDPR) and Japan's Act on the Protection of Personal Information). To the extent such data contains personal or pseudonymized information, its handling is governed by the [Privacy Policy](https://vrclarity.net/docs/privacy).

### 2.2 Data Usage Rights
- **World Creators**: Can view statistics about their worlds through the VRClarity dashboard
- **VRClarity**: May use transmitted data for the following purposes:
  - Providing and improving the service
  - Publishing anonymized and aggregated statistics
  - Research and analysis
  - Introducing the service and publishing statistics

### 2.3 Third-Party Data Sharing
VRClarity may provide anonymized and aggregated data to third parties as statistical information. However, such data will not include personally identifiable information or information that identifies specific worlds.

## 3. Prohibited Activities

The following activities are **strictly prohibited** when using the SDK. Violations may result in SDK suspension, data deletion, and/or legal action.

### 3.1 Endpoint Modification
- Changing the endpoint URL to which the SDK transmits data
- Modifying the SDK to send data to servers other than VRClarity's
- Using network settings or proxies to redirect data transmission

### 3.2 Data Misappropriation and Unauthorized Use
- Providing, selling, or transferring transmitted data to third parties
- Modifying the SDK to use it as a custom data transmission system
- Unauthorized use of VRClarity's infrastructure
- Mass data extraction through scraping or crawling

### 3.3 SDK Abuse
- Publishing or transferring obtained encryption keys or API keys to third parties
- Disabling or circumventing security features to misuse VRClarity's service

### 3.4 Unauthorized Access and Service Disruption
- Attempting unauthorized access to VRClarity servers
- DDoS attacks or other service disruption activities
- Intentionally transmitting false or malicious data
- Generating excessive server load through excessive requests

### 3.5 Trademark and Intellectual Property Infringement
- Unauthorized use of VRClarity's trademarks or logos
- Redistributing or reselling the SDK (except through official VPM registry distribution)
- Removing or modifying copyright notices in the SDK

## 4. Relationship with License

While the SDK's source code is provided under the MIT License, **these Terms govern data usage and service usage independently of the license.**

- **MIT License**: Grants rights to modify and redistribute code
- **Terms of Service**: Define conditions for data ownership and service usage

Even when modifying or redistributing code under the license terms, **these Terms apply when using VRClarity's service.**

## 5. Service Changes and Termination

VRClarity reserves the right to take the following actions without prior notice:

- Changing API endpoints
- Modifying data collection specifications
- Temporarily suspending or terminating the service
- Updating these Terms

We will make reasonable efforts to provide advance notice for significant changes, but may make changes without notice in emergency situations or for security reasons.

## 6. Disclaimer

- **Data Accuracy**: We do not guarantee the accuracy of transmitted data
- **Service Availability**: We do not guarantee continuous service availability
- **Limitation of Liability**: To the extent permitted by law, we are not liable for damages arising from the use of the SDK

The SDK is provided "as is" without any warranties, express or implied. However, these disclaimers and limitations do not apply in cases of our willful misconduct or gross negligence, or where exemption or limitation of liability is not permitted under the Consumer Contract Act or other applicable laws.

## 7. Governing Law and Jurisdiction

These Terms are governed by the laws of Japan. Any disputes related to these Terms shall be subject to the exclusive jurisdiction of the district court having jurisdiction over the operator's principal place of business as the court of first instance.

The foregoing does not limit any mandatory consumer-protection rights available to you under the laws of your place of habitual residence.

## 8. Contact

For questions about these Terms, please contact:

- **Contact Form**: [https://vrclarity.net/contact](https://vrclarity.net/contact)
- **Email**: hello@vrclarity.net

---

**Change Log**

- **v1.4.0 (2026-06-23)**: Narrowing of the data-ownership clause and clarification of usage purposes
  - Revised Section 2.1 to limit "ownership" to rights in the aggregated/statistical datasets and works derived therefrom, rather than blanket ownership of all raw data
  - Changed the Section 2.2 usage purpose "Marketing and promotional activities" to "Introducing the service and publishing statistics"
- **v1.3.0 (2026-06-22)**: Visitor notice and data-ownership clarification
  - Added Section 1.1 "Notice to Visiting Players," stating the strong recommendation to inform visitors of data collection by maintaining the Notice Panel (aligned with Article 4(3) of the web Service Terms and Section 2.3 of the Privacy Policy)
  - Added to Section 2.1 that the "ownership" under these Terms concerns the aggregated/statistical datasets and does not limit data subjects' rights under data protection laws (GDPR, Japan's APPI, etc.), resolving the tension between the ownership claim and the handling of pseudonymized information (aligned with the Privacy Policy)
- **v1.2.0 (2026-06-22)**: Alignment with the service Terms of Service
  - Changed the court of jurisdiction in Section 7 from "Tokyo District Court" to "the district court having jurisdiction over the operator's principal place of business," unifying it with the web service Terms (Article 13)
  - Added a clause in Section 7 clarifying that mandatory consumer-protection rights are not limited
  - Limited the Section 6 disclaimer to "the extent permitted by law" and added that the exemptions do not apply in cases of willful misconduct, gross negligence, or where prohibited by the Consumer Contract Act or other laws (addressing the risk that a blanket disclaimer is void; aligned with Article 8 of the web Terms)
- **v1.1.1 (2026-06-13)**: Contact information update
  - Replaced the contact channel with the contact form (https://vrclarity.net/contact) and email (hello@vrclarity.net)
- **v1.1.0 (2026-04-13)**: Terminology and policy clarification
  - Unified terminology from "collected" to "transmitted"
  - Revised Section 3.3 to align with MIT License (removed reverse engineering prohibition)
- **v1.0.0 (2026-03-27)**: Initial release

---

Made with ❤️ by studio peipeiko
