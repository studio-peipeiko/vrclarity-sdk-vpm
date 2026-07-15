# VRClarity SDK セットアップガイド

VRChat ワールドの利用状況に関する匿名の集計値を、VRClarity に送信する SDK です。個人を特定する情報は取得・送信しません。

> 📖 **セットアップ手順・メトリクスの詳細・トラブルシューティングは、常に最新のオンラインドキュメントを参照してください。**
> 本ファイルは概要のみを記載し、詳細は以下に集約しています（重複メンテを避けるため）。

## ドキュメント

- **SDK 導入ガイド（手順の全文・図解）**: https://vrclarity.net/docs/sdk-guide
- **SDK の概要（計測データの種類と見方）**: https://vrclarity.net/docs/sdk
- **プライバシーポリシー**: https://vrclarity.net/docs/privacy
- **サービス利用規約**: https://vrclarity.net/docs/terms
- **SDK 利用規約（データの取り扱い・禁止事項等）**: [ToS.md](../../../ToS.md)

## クイックスタート

1. VCC に以下のレジストリを追加：
   ```
   https://studio-peipeiko.github.io/vrclarity-sdk-vpm/index.json
   ```
2. **Manage Project** から **VRClarity SDK** をインストール
3. [VRClarity ダッシュボード](https://vrclarity.net) の「SDK APIキー管理」で **Key ID** と **Encryption Key** を発行
4. ヒエラルキー右クリック → **VRClarity → Create Tracker**（Tracker と Notice Panel が自動配置されます）
5. Inspector に Key ID / Encryption Key を入力
6. **アップロード直前に Inspector の「Enable Tracking」にチェック**を入れ、通常どおりワールドをビルド＆アップロード（未チェックの間は送信されません）

> データがダッシュボードに反映されるまで最大約 1 時間かかります。

詳しい手順・図解・トラブルシューティングは [SDK 導入ガイド](https://vrclarity.net/docs/sdk-guide) を参照してください。

## 動作要件

- Unity 2022.3 以上
- VRChat SDK Worlds 3.7.0 以上（UdonSharp 同梱）

## ライセンス

MIT License（[LICENSE](../../../LICENSE)）
