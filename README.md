# 🏥 Hospitalia AI

**An end-to-end Smart Healthcare Ecosystem** that unifies clinical consultations, diagnostic labs, pharmacy logistics, payments, and AI-assisted care in a single web platform.

Built with **.NET** (Web API) · **React** · **Microsoft SQL Server**

---

## ✨ Overview

Hospitalia AI connects patients, doctors, diagnostic labs, and pharmacies on one platform. AI modules sit on top of the clinical workflow to speed up triage, documentation, and diagnostics, while a **human-in-the-loop** design keeps licensed doctors in control of every medical decision.

## 🚀 Key Features

### Clinical & Care
- **Doctor discovery & booking**: specialties, schedules, leaves, appointments, and patient reviews with auto-calculated doctor ratings
- **Electronic Medical Records (EMR)**: encounters with SOAP notes and ICD-10 coding
- **Digital prescriptions**: structured medication items with drug master data and drug-interaction checks
- **Patient profiles**: allergies and medical history

### Diagnostics & Pharmacy
- **Lab network**: lab test catalog, lab orders, lab staff, and phlebotomist home-visit support
- **Pharmacy network**: batch-level inventory, prescription-based orders, and delivery tracking

### Platform & Security
- **Role-based access**: patient, doctor, lab staff, pharmacy staff, and super admin
- **Verification workflow**: document review for doctors, labs, and pharmacies (syndicate ID, commercial registry, clinic license)
- **Transactions**: gross amount, platform fee, and net payout with integrity constraints
- **Immutable audit logs**: tamper-proof trail protected by database triggers

## 🤖 AI Modules

| Module | What it does |
|---|---|
| **AI Medical Triage** | Patients describe symptoms in natural language; the agent returns an urgency tier (`emergency` / `urgent` / `routine`), a suggested specialty, and structured reasoning |
| **AI Radiology (HITL)** | Analyzes X-rays, CTs, and skin lesions with a confidence score, risk level, and **Grad-CAM heatmaps** for explainability. Results stay *unverified* until a licensed doctor reviews them |
| **AI Clinical Scribe** | Transcribes consultation audio and auto-fills SOAP notes (Subjective, Objective, Assessment, Plan) |
| **Prescription OCR** | Digitizes handwritten paper prescriptions and matches medications against the drug master list |

```
Symptoms → AI Triage → Specialty Booking → Consultation → AI Scribe → EMR → e-Prescription → Pharmacy
                                                  ↓
                                    Scan Upload → AI Vision → Doctor Verification → Final Report
```

## 🧰 Tech Stack

| Layer | Technology |
|---|---|
| **Frontend** | React, Tailwind CSS |
| **Backend** | .NET (ASP.NET Core Web API) |
| **Database** | Microsoft SQL Server 2019/2022, T-SQL |
| **AI / ML** | LLM APIs (OpenAI / Gemini), Whisper speech-to-text, PyTorch (DenseNet121 / ResNet50) with Grad-CAM, OCR (Google Cloud Vision / Tesseract) |

## 🗂️ Database

The full production schema lives in the architecture guide: `hospitalia_ai_architecture_guide.md`. It covers:

1. Identity & governance
2. Doctors, scheduling & appointments
3. EMR & AI triage
4. Diagnostic labs
5. Prescriptions & AI radiology
6. Pharmacies & orders
7. Transactions & audit logging
8. Indexes and business-logic triggers

## 📁 Project Structure

```
hospitalia-ai/
├── backend/        # .NET Web API
├── frontend/       # React web app
├── database/       # T-SQL schema scripts
└── docs/           # Architecture & design docs
```

## ⚙️ Getting Started

```bash
# 1. Clone
git clone https://github.com/<your-username>/hospitalia-ai.git
cd hospitalia-ai

# 2. Database: run the T-SQL script in SSMS (SQL Server 2019/2022)

# 3. Backend
cd backend
dotnet restore
dotnet run

# 4. Frontend
cd ../frontend
npm install
npm run dev
```

## ⚠️ Medical Disclaimer

Hospitalia AI is a software project. AI outputs are **decision-support only** and must always be reviewed by a licensed medical professional. It is not a substitute for professional medical advice, diagnosis, or treatment.

## 📄 License

MIT. See [LICENSE](LICENSE) for details.
