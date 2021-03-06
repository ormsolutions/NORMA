// Current language
var curLang = "";

window.onload = hsLanguageFilteredBodyLoad;

function hsLanguageFilteredBodyLoad()
{

    var i;
    var b;
    var l;

	resizeBan();

	document.body.onclick = bodyClick;
	document.body.onresize = resizeBan;
	window.onbeforeprint = hsBeforePrint;
	window.onafterprint = hsAfterPrint;
	window.onunload = windowUnload;

    // Check the context window for current language.
    try
    {
        for (i=1; i< window.external.ContextAttributes.Count; i++)
        {
            if(window.external.ContextAttributes(i).Name.toUpperCase()=="DEVLANG")
            {
                b = window.external.ContextAttributes(i).Values.toArray();
                curLang = b[0].toUpperCase();
            }
        }
    }
    catch(e)
    {
    }

    if (curLang != null)
    {
        if (curLang.indexOf("VB") != -1)
        {
            curLang = "VB";
        }
        if (curLang.indexOf("VC") != -1)
        {
            curLang = "CPP";
        }
        if (curLang.indexOf("C++") != -1)
        {
            curLang = "CPP";
        }
        if (curLang.indexOf("CSHARP") != -1)
        {
            curLang = "CS";
        }
        if (curLang.indexOf("JSCRIPT") != -1)
        {
            curLang = "JScript";
        }
    }

    if (curLang == null)
    {
        l = "";
        var multipleLang = false;
        var cLang = "";

        try
        {
            l = window.external.Help.FilterQuery.toUpperCase();
        }
        catch(e)
        {
        }

        if (l.indexOf("VB") != -1)
        {
            cLang = "VB";
        }

        if (l.indexOf("VC") != -1)
        {
            if (cLang != null)
            {
                multipleLang = true;
            }
            cLang = "CPP";
        }

        if (l.indexOf("C#") != -1)
        {
            if (cLang != null)
            {
                multipleLang = true;
            }
            cLang = "CS";
        }

        if (l.indexOf("CSHARP") != -1)
        {
            if (cLang != null)
            {
                multipleLang = true;
            }
            cLang = "CS";
        }

        if (l.indexOf("JSCRIPT") != -1)
        {
            if (cLang != null)
            {
                multipleLang = true;
            }
            cLang = "JScript";
        }

        if (l.indexOf("JSHARP") != -1)
        {
            if (cLang != null)
            {
                multipleLang = true;
            }
            cLang = "JSHARP";
        }

        if (multipleLang == false)
        {
            curLang = cLang;
        }
    }

	loadLanguages();
    configureLanguageCheckboxes();
    displayLanguages();

	// make body visible, now that we're ready to render
	document.body.style.display = "";
}

function windowUnload()
{
	saveLanguages();
}

/* Language Filtering */

var languageNames;
var languageStates;

function configureLanguageCheckboxes()
{
    var checkbox;
    var i;

    for(i=0;i<languageNames.length;i++)
    {
        checkbox = documentElement(languageNames[i] + "Checkbox");

        if(languageStates[i] == "on")
        {
            checkbox.checked = true;
        }
        else
        {
            checkbox.checked = false;
        }
    }
}

function setLanguage(key)
{
    var languageName = key.id.substring(0,key.id.indexOf("Checkbox"));

    if(getLanguageState(languageName) == "on")
    {
        // Always have at least one selected
        if(getLanguageTickedCount() == 1)
        {
            key.checked = true;
            return;
        }

        setLanguageState(languageName,"off");
        key.checked = false;
    }
    else
    {
        setLanguageState(languageName,"on");
        key.checked = true;
    }

    // Update the content to reflect the new language filter
    displayLanguages();
}

function displayLanguages()
{
    var pres = document.getElementsByTagName("DIV");
    var pre;
    var found;
    var languageName;

    if (pres)
    {
        for (var iPre = 0; iPre < pres.length; iPre++)
        {
            pre = pres[iPre];

            if (pre.id && pre.className)
            {
                if (pre.className == "LanguageSpecific")
                {
                    found = true;

                    for(var i=0;i<languageNames.length;i++)
                    {
                        if(languageStates[i] == "off")
                        {
                            languageName = languageNames[i].toUpperCase();

                            // For each language specific element except the Syntax, treat CPP2005 as CPP
                            if (languageName == "CPP2005" && pre.id.toUpperCase().indexOf("SYNTAX") == -1)
                            {
                                languageName = "CPP";
                            }

                            if (pre.id.toUpperCase().indexOf(languageName) == (pre.id.length - languageName.length))
                            {
                                found = false;
                                break;
                            }
                        }
                    }

                    if(found)
                    {
                        pre.style.display = "block";
                    }
                    else
                    {
                        pre.style.display = "none";
                    }
                }
            }
        }
    }

    changeLanguageFilterLabel();
}

function getLanguageState(LanguageName)
{
    for(var i=0;i<languageNames.length;i++)
    {
        if(languageNames[i] == LanguageName)
        {
            return(languageStates[i]);
        }
    }
}

function setLanguageState(LanguageName,Value)
{
    for(var i=0;i<languageNames.length;i++)
    {
        if(languageNames[i] == LanguageName)
        {
            languageStates[i] = Value;
        }
    }
}

function getLanguageTickedCount()
{
    var tickedCount=0;
    var labelElement;

    for(var i=0;i<languageNames.length;i++)
    {
        if(languageStates[i] == "on")
        {
            tickedCount++;
        }
    }

    return(tickedCount);
}

function changeLanguageFilterLabel()
{
    var tickedCount=0;
    var labelElement;
    var languageName;

    if(!document.getElementById("showAllLabel"))
    {
        return;
    }

    for(var i=0;i<languageNames.length;i++)
    {
        if(languageStates[i] == "on")
        {
            tickedCount++;
        }

        labelElement = documentElement(languageNames[i] + "label");

        if(labelElement != null)
        {
            labelElement.style.display = "none";
        }
    }

    document.getElementById("showAllLabel").style.display = "none";
    document.getElementById("multipleLabel").style.display = "none";

    if(tickedCount == languageNames.length)
    {
        document.getElementById("showAllLabel").style.display = "inline";
    }
    else if ((tickedCount > 1) && (tickedCount < languageNames.length))
    {
        if((tickedCount == 2) && (getLanguageState("VB") == "on") && (getLanguageState("vbUsage") == "on"))
        {
            document.getElementById("VBLabel").style.display = "inline";
        }
        else
        {
            document.getElementById("multipleLabel").style.display = "inline";
        }
    }
    else if (tickedCount == 1)
    {
        for(i=0;i<languageNames.length;i++)
        {
            if(languageStates[i] == "on")
            {
                if(languageNames[i] == "vbUsage")
                {
                    languageName = "VB";
                }
                else
                {
                    languageName = languageNames[i];
                }

                document.getElementById(languageName + "Label").style.display = "inline";
            }
        }
    }
}

function loadLanguages()
{
    var languageName;
    var language;
    var allNull;

    // Build an array of this pages language names and state
    languageNames = new Array();
    languageStates = new Array();

    var elements = document.getElementsByName("languageFilter");

    allNull = true;

    for(var i=0;i<elements.length;i++)
    {
        var input = elements[i];

        languageNames[i] = input.id.substring(0,input.id.indexOf("Checkbox"));
        var value = load("lang_" + languageNames[i]);

        if(value == null)
        {
            languageStates[i] = "on";
        }
        else
        {
            allNull = false;
            languageStates[i] = value;
        }
    }

        // If no language preference has been established and we have an indicator of the current
        //  language, set the languages filtered to only the current language
    if(allNull && curLang.length > 0)
    {
        for(i=0;i<elements.length;i++)
        {
            if(languageNames[i].toUpperCase() == curLang.toUpperCase())
            {
                languageStates[i] = "on";
            }
            else
            {
                languageStates[i] = "off";
            }
        }
    }
}

function saveLanguages()
{
    if(languageNames)
    {
        for(var i=0;i<languageNames.length;i++)
        {
            var value = languageStates[i];
            save("lang_" + languageNames[i], value);
        }
    }
}

/* End Language Filtering */

/* To hide all popup boxes */
function hideBoxes()
{
    var pres = document.getElementsByTagName("DIV");
    var pre;

    if (pres)
    {
        for (var iPre = 0; iPre < pres.length; iPre++)
        {
            pre = pres[iPre];

            if (pre.className)
            {
                if (pre.className == "hspopupbubble")
                {
                    pre.style.visibility = "hidden";
                }
            }
        }
    }
}

/* Show a popup box */
function showBox(Name,e,button)
{
    if (window.event)
    {
        e = window.event;
    }

    hideBoxes();

    if(!button)
    {
    	button = sourceElement(e);
    }
    cancelEvent(e);
    var div = documentElement(Name);

    if (div && button)
    {
        div.style.height = "";
        div.style.left = (getElementPosition(button).left) + "px";
        div.style.top = (getElementPosition(button).top + button.offsetHeight) + "px";
        div.style.visibility = "visible";
    }
}

function bodyClick(e)
{
    if (window.event)
    {
        e = window.event;
    }
    if(sourceElement(e))
    {
        if(sourceElement(e).tagName != "INPUT")
        {
            if (sourceElement(e).className != "languageFilter")
            {
            hideBoxes();
            resizeBan();
            };
        }
    }
}
