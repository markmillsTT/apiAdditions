#include <string>
#include "stdafx.h"
#include "TTAPIFunctions.h"


TTAPIFunctions::TTAPIFunctions(System::String ^ username, System::String ^ password)
{
	m_username = username;
	m_password = password;
}

void TTAPIFunctions::Start()
{
	m_disp = TTAPI::Dispatcher::AttachWorkerDispatcher();
	m_disp->BeginInvoke(gcnew System::Action(this, &TTAPIFunctions::Init));
	m_disp->Run();
}

void TTAPIFunctions::Init()
{
	TTAPI::ApiInitializeHandler ^ handler = gcnew TTAPI::ApiInitializeHandler(this, &TTAPIFunctions::handleLogin);
	TTAPI::TTAPI::CreateUniversalLoginTTAPI(TTAPI::Dispatcher::Current, m_username, m_password, handler);
}

void TTAPIFunctions::handleLogin(TTAPI::TTAPI ^ api, TTAPI::ApiCreationException ^ ex)
{
	if (ex == nullptr)
	{
		m_apiInstance = (TTAPI::UniversalLoginTTAPI ^)api;
		m_apiInstance->AuthenticationStatusUpdate += gcnew System::EventHandler<TradingTechnologies::TTAPI::AuthenticationStatusUpdateEventArgs ^>(this, &TTAPIFunctions::OnAuthenticationStatusUpdate);
		m_apiInstance->Start();
		(*m_apiInstance).AuthenticationStatusUpdate
	}
}

TTAPIFunctions::~TTAPIFunctions()
{
}

void TTAPIFunctions::OnAuthenticationStatusUpdate(System::Object ^sender, TradingTechnologies::TTAPI::AuthenticationStatusUpdateEventArgs ^e)
{
	if (e->Status->IsSuccess)
	{
		// lookup an instrument
		System::Console::WriteLine("TT Login suceeded, looking up instrument");
		m_req = gcnew TTAPI::InstrumentLookupSubscription(m_apiInstance->Session, TTAPI::Dispatcher::Current, TTAPI::ProductKey(TTAPI::MarketKey::Cme, TTAPI::ProductType::Future, "ES"), "Mar16");
		m_req->Update += gcnew System::EventHandler<TradingTechnologies::TTAPI::InstrumentLookupSubscriptionEventArgs ^>(this, &TTAPIFunctions::OnUpdate);
		m_req->Start();
	}
}


void TTAPIFunctions::OnUpdate(System::Object ^sender, TradingTechnologies::TTAPI::InstrumentLookupSubscriptionEventArgs ^e)
{
	if (e->Instrument != nullptr && e->Error == nullptr)
	{
		// Instrument was found
		System::Console::WriteLine("Found: {0}", e->Instrument->Name);

		// Subscribe for Inside Market Data
		m_ps = gcnew TTAPI::PriceSubscription(e->Instrument, TTAPI::Dispatcher::Current);
		m_ps->Settings = gcnew TTAPI::PriceSubscriptionSettings(TTAPI::PriceSubscriptionType::InsideMarket);
		m_ps->FieldsUpdated += gcnew TradingTechnologies::TTAPI::FieldsUpdatedEventHandler(this, &TTAPIFunctions::OnFieldsUpdated);
		m_ps->Start();
	}
	else if (e->IsFinal)
	{
		// Instrument was not found and TT API has given up looking for it
		System::Console::WriteLine("Cannot find instrument: {0}", e->Error->Message);
	}
}


void TTAPIFunctions::OnFieldsUpdated(System::Object ^sender, TradingTechnologies::TTAPI::FieldsUpdatedEventArgs e)
{
	if (e.Error == nullptr)
	{
		if (e.UpdateType == TTAPI::UpdateType::Snapshot)
		{
			// Received a market data snapshot
			System::Console::WriteLine("Market Data Snapshot:");

			for each(TTAPI::FieldId id in e.Fields->GetFieldIds())
			{
				if (id == TTAPI::FieldId::SettlementPrice)
				{
					System::Console::WriteLine("Settlement Price");
				}
				System::Console::WriteLine("    {0} : {1}", id.ToString(), e.Fields[id]->FormattedValue);
			}
		}
		else
		{
			// Only some fields have changed
			System::Console::WriteLine("Market Data Update:");

			for each(TTAPI::FieldId id in e.Fields->GetChangedFieldIds())
			{
				if (id == TTAPI::FieldId::SettlementPrice)
				{
					System::Console::WriteLine("Settlement Price Changed");
				}
				System::Console::WriteLine("    {0} : {1}", id.ToString(), e.Fields[id]->FormattedValue);
			}
			TTAPI::TradingStatus ts = e.Fields->GetSeriesStatusField()->Value;
		}
	}
	else
	{
		if (e.Error->IsRecoverableError == false)
		{
			System::Console::WriteLine("Unrecoverable price subscription error: {0}", e.Error->Message);
			//Dispose();
		}
	}
}
