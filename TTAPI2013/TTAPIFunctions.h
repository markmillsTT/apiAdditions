using namespace TradingTechnologies;

ref class  TTAPIFunctions
{
public:
	TTAPIFunctions(System::String ^ username, System::String ^ password);
	~TTAPIFunctions();
	void Init();
	void Start();

private:
	TTAPI::UniversalLoginTTAPI ^ m_apiInstance;
	TTAPI::WorkerDispatcher ^ m_disp;
	void handleLogin(TTAPI::TTAPI ^ api, TTAPI::ApiCreationException ^ ex);	


	TTAPI::InstrumentLookupSubscription ^ m_req;
	TTAPI::PriceSubscription ^ m_ps;
	static TTAPI::OrderProfile ^ m_op;

	System::String ^ m_username;
	System::String  ^ m_password;

	void OnAuthenticationStatusUpdate(System::Object ^sender, TradingTechnologies::TTAPI::AuthenticationStatusUpdateEventArgs ^e);
	void OnUpdate(System::Object ^sender, TradingTechnologies::TTAPI::InstrumentLookupSubscriptionEventArgs ^e);
	void OnFieldsUpdated(System::Object ^sender, TradingTechnologies::TTAPI::FieldsUpdatedEventArgs e);
};