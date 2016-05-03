/***************************************************************************
 *    
 *      Copyright (c) 2012 Trading Technologies International, Inc.
 *                     All Rights Reserved Worldwide
 *
 *        * * *   S T R I C T L Y   P R O P R I E T A R Y   * * *
 *
 * WARNING:  This file is the confidential property of Trading Technologies
 * International, Inc. and is to be maintained in strict confidence.  For
 * use only by those with the express written permission and license from
 * Trading Technologies International, Inc.  Unauthorized reproduction,
 * distribution, use or disclosure of this file or any program (or document)
 * derived from it is prohibited by State and Federal law, and by local law
 * outside of the U.S. 
 *
 ***************************************************************************/

#include "../stdafx.h"
#include "DropHandlerSink.h"

/* Create the instance of the TTDropHandler
 */
CDropHandlerSink::CDropHandlerSink()
{
	// clear out our callback till we pass it in
	m_pCallback=NULL;

	// create the notification object
	HRESULT hr=m_pDropHandlerObj.CreateInstance(__uuidof(XTAPI::TTDropHandler));

	if (hr==S_OK)
	{
		// created OK, connect to it
		hr=DispEventAdvise(m_pDropHandlerObj);
	}
}

/* Destroy the TTDropHandler 
 */
CDropHandlerSink::~CDropHandlerSink()
{
	if (m_pDropHandlerObj)
	{
		// remove our connection point
		DispEventUnadvise(m_pDropHandlerObj);
	}

	// de-ref the SRC
	m_pDropHandlerObj=NULL;
}

/* Set the callback mechanism
 */
void CDropHandlerSink::SetCallback(CNotify*pCallback)
{
	// save the callback pointer
	m_pCallback=pCallback;
}

/* Assign the OnNotifyDrop event
 */
HRESULT CDropHandlerSink::OnNotifyDrop()
{
	// Connection point callback function
	if (m_pCallback)
	{
		// pass this onto the external event consumer
		m_pCallback->CDropHandlerSink_NotifyDrop(this);
	}
	return S_OK;
}