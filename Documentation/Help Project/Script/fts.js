function documentElement(id){
	return document.getElementById(id);
}
		
function btnSearch_onclick() 
{
	// hide search div
	documentElement("divSearch").style.display = "none";
	
	// show progress div
	documentElement("divSearching").style.display = "";
	
	// do search
	var bResult = DoSearch();

	// hide progress div
	documentElement("divSearching").style.display = "none";
	
	// hide search div
	documentElement("divSearch").style.display = "";	

	// show results
	if (bResult == true)
	{
		// show resulta div
		documentElement("divResults").style.display = "";
	}

}

function btnNewSearch_onclick()
{
	document.execCommand("refresh");
}

function DoSearch()
{
	// get search string
	var strSearchString = new String(documentElement("txtSearch").value);
	if (strSearchString.length == 0)
	{
		return false;
	}
	
	// make lowercase for easy comparison later on
	strSearchString = strSearchString.toLowerCase();
	
	// tokenise
	var aTokens = strSearchString.split(" ");
	
	// build keywords array & query string
	var strQuery = new String();
	var bLastOperator = true;
	var bLastKeyword = false;
	var nKeyword = 0;
	var aKeywords = new Array();
	for (var nToken = 0; nToken < aTokens.length; nToken++)
	{
		if (aTokens[nToken].length > 0)
		{
			var strTok = new String(aTokens[nToken]);
			
			// strip out any delimiters & whitespace
			strTok = strTok.replace(new RegExp("\'", "ig"), "");
			strTok = strTok.replace(new RegExp("\"", "ig"), "");
			strTok = strTok.replace(new RegExp(",", "ig"), "");
			
			// add to array
			if ((strTok == "or") || (strTok == "and"))
			{
				// only add it if not just had an operator
				if (bLastOperator == false)
				{
					// add to the query string
					if (strTok == "or")
					{
						strQuery += " || ";
					}
					else if (strTok == "and")
					{
						strQuery += " && ";
					}
					bLastOperator = true;
					bLastKeyword = false;
				}
			}
			else
			{
				// we just had a keyword ?
				if (bLastKeyword)
				{
					// add default OR operator
					strQuery += " || ";
				}
				
				// add to query string
				//strQuery += "(strKey.indexOf(";
				//strQuery += "\"";
				
				strQuery += ("(strKey.indexOf(\"" + strTok + "\") >= 0)");
				bLastOperator = false;
				bLastKeyword = true;
				
				// add to keywords array
				aKeywords[nKeyword] = strTok;
				nKeyword++;
			}
		}
	}
	if (aKeywords.length == 0)
	{
		return false;
	}
	
	// load files & stopwords arrays
	var aIndexFiles = new Array();
	var aIndexFileTitles = new Array();
	var aIndexStopWords = new Array();
	BuildIndexArray(aIndexFiles, aIndexFileTitles, aIndexStopWords);
	
	// build list of files containing keywords
	var aFiles = new Array();
	var aFileTitles = new Array();
	var aFileKeywords = new Array();
	LoadFilesWithKeywords(aIndexFiles, aIndexFileTitles, aIndexStopWords, aKeywords, aFiles, aFileTitles, aFileKeywords);
	
	// build results list
	var aResults = BuildResultsArray(strQuery, aFileKeywords);
	
	// write results to document
	OutputResults(aResults, aFiles, aFileTitles);
	
	// return success
	return true;
}

function LoadFilesWithKeywords(aIndexFiles, aIndexFileTitles, aIndexStopWords, aKeywords, aFiles, aFileTitles, aFileKeywords)
{
	// build lookup arrays
	for (var nStopWord = 0; nStopWord < aKeywords.length; nStopWord++)
	{
		// find stop word in index
		if (aIndexStopWords["_" + aKeywords[nStopWord]] != undefined)
		{		
			// get indices of files containing word
			var strIndices = new String(aIndexStopWords["_" + aKeywords[nStopWord]]);
			
			// tokenize indices
			var aIndices = strIndices.split(",");
			for (var nIndex = 0; nIndex < aIndices.length; nIndex++)
			{
				// add to array
				var nFileIndex = new Number(aIndices[nIndex]);
				if (aFileKeywords[nFileIndex] == undefined)
				{
					aFileKeywords[nFileIndex] = aKeywords[nStopWord];
				}
				else
				{
					aFileKeywords[nFileIndex] += ("," + aKeywords[nStopWord]);
				}
			}
		}
	}
	
	// get filenames for indices
	for (var nFile = 0; nFile < aFileKeywords.length; nFile++)
	{
		if (aFileKeywords[nFile] != undefined)
		{
			// find stop word in index
			if (aIndexFiles[nFile] != undefined)
			{
				aFiles[nFile] = aIndexFiles[nFile];
				aFileTitles[nFile] = aIndexFileTitles[nFile];
			}
		}
	}
}

function BuildResultsArray(strQuery, aFileKeywords)
{
	var aResults = new Array();
	
	for (var nIndex = 0; nIndex < aFileKeywords.length; nIndex++)
	{
		if (aFileKeywords[nIndex] != undefined)
		{
			var strKey = aFileKeywords[nIndex];
			if (eval(strQuery) == true)
			{
				aResults[nIndex] = nIndex;
			}
		}
	}
	return aResults;
}

function OutputResults(aResults, aFiles, aFileTitles)
{
	// init table html
	var strTable = "<p>&nbsp;</p><font size=2><b>Search Results</b></font><p><blockquote>";
	
	// add results to table
	var nTotal = 0;
	for (var nResult = 0; nResult < aResults.length; nResult++)
	{
		if (aResults[nResult] != undefined)
		{
			// add row text
			strTable += "<a target='body' href='" + aFiles[aResults[nResult]] + "'>" + aFileTitles[aResults[nResult]] + "</a><br/>";
			
			// incr total
			nTotal++;
		}
	}

	// add footer
	strTable += "</blockquote></p><p>" + nTotal + " result(s) found.</p>";
					
	// set it
	documentElement("divResults").innerHTML = strTable; //+ documentElement("divResults").innerHTML;
}

		
